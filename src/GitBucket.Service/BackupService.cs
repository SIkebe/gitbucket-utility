using System.Diagnostics;
using GitBucket.Core;
using LibGit2Sharp;
using Npgsql;

namespace GitBucket.Service;

public interface IBackupService
{
    int Backup(BackupOptions options, string connectionString);
}

/// <summary>
/// Port of https://github.com/gitbucket/gitbucket/wiki/Backup for C# & PostgreSQL.
/// </summary>
public class BackupService(IConsole console) : IBackupService
{
    private readonly IConsole _console = console ?? throw new ArgumentNullException(nameof(console));

    /// <summary>
    /// To keep integrity as its maximum possible, the database export and git backups must be done in the shortest possible timeslot.
    /// Thus we will:
    ///  - clone new repositories into backup directory
    ///  - update them all a first time, so that we gather updates
    ///  - export the database
    ///  - update all repositories a second time, this time it should be ultra-fast
    /// </summary>
    /// <param name="options">The BackupOptions.</param>
    /// <param name="connectionString">The connectionString.</param>
    /// <returns>Return code.</returns>
    public int Backup(BackupOptions options, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(options);

        Directory.CreateDirectory(options.Destination);
        Directory.CreateDirectory(options.DestinationRepositoriesDir);

        CloneRepositories(options);

        _console.WriteLine("Update repositories: phase 1");
        UpdateRepositories(options);
        _console.WriteLine("Update repositories: phase 1, terminated");

        if (PgDumpAvailable(options))
        {
            DumpDatabase(options, connectionString);
        }

        // Export the GitBucket configuration
        _console.WriteLine("Configuration backup");
        if (File.Exists(options.GitBucketHomeConfigurationFile))
        {
            File.Copy(options.GitBucketHomeConfigurationFile, options.DestinationConfigurationFile, overwrite: true);
        }

        // Export the GitBucket database configuration
        _console.WriteLine("Database configuration backup");
        File.Copy(options.GitBucketHomeDatabaseConfigurationFile, options.DestinationDatabaseConfigurationFile, overwrite: true);

        // Export the GitBucket data directory (avatars, ...)
        // data directory exists only if there are some files uploaded by users.
        if (Directory.Exists(options.GitBucketHomeDataDir))
        {
            _console.WriteLine("Avatars backup");
            CopyEntireDirectory(options.GitBucketHomeDataDir, options.DestinationDataDir);
        }

        _console.WriteLine("Plugins backup");
        CopyEntireDirectory(options.GitBucketPluginsDataDir, options.DestinationPluginsDir);

        _console.WriteLine("Update repositories: phase 2");
        UpdateRepositories(options);
        _console.WriteLine("Update repositories: phase 2, terminated");

        _console.WriteLine($"Update process ended, backup available under: {options.DestinationRepositoriesDir}");
        return 0;
    }

    /// <summary>
    /// Update a cloned folder, the update is down toward the latest state of its default remote.
    /// As "git remote update" is not implemented in LibGit2Sharp yet, using "git fetch" instead.
    /// https://github.com/libgit2/libgit2sharp/issues/1466
    /// </summary>
    /// <param name="options">The BackupOptions.</param>
    private void UpdateRepositories(BackupOptions options)
    {
        foreach (var owener in Directory.EnumerateDirectories(options.DestinationRepositoriesDir))
        {
            foreach (var repositoryPath in Directory.EnumerateDirectories(owener, "*.git", SearchOption.TopDirectoryOnly))
            {
                _console.WriteLine($"updating {repositoryPath}");
                using var repo = new LibGit2Sharp.Repository(repositoryPath);
                var remote = repo.Network.Remotes["origin"];
                var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
                Commands.Fetch(repo, remote.Name, refSpecs, null, null);
            }
        }
    }

    /// <summary>
    /// Clone all repositories, comments, diff, lfs-files, releases into repositoryBackupFolder.
    /// </summary>
    /// <param name="options">The BackupOptions.</param>
    private void CloneRepositories(BackupOptions options)
    {
        _console.WriteLine("Starting clone process");

        foreach (var ownerPath in Directory.EnumerateDirectories(options.GitBucketHomeRepositoriesDir, "*", SearchOption.TopDirectoryOnly))
        {
            var owner = new DirectoryInfo(ownerPath).Name;
            Directory.CreateDirectory(Path.Combine(options.DestinationRepositoriesDir, owner));

            var repositories = Directory.EnumerateDirectories(ownerPath, "*", SearchOption.TopDirectoryOnly);
            foreach (var repositoryPath in repositories)
            {
                var repository = new DirectoryInfo(repositoryPath).Name;
                if (repository.Contains(".git", StringComparison.OrdinalIgnoreCase))
                {
                    // Backup git bare repository
                    CreateClone(owner, repository, options);
                }
                else
                {
                    // Backup comments, diff, lfs, releases
                    CopyEntireDirectory(repositoryPath, Path.Combine(options.DestinationRepositoriesDir, owner, repository));
                }
            }
        }

        _console.WriteLine("All repositories cloned");
    }

    /// <summary>
    /// Copy a directory including all sub-directories. All files will be overwritten.
    /// </summary>
    /// <param name="sourceDirPath">The source directory path to copied from.</param>
    /// <param name="destDirPath">The destination directory path to copied to.</param>
    private void CopyEntireDirectory(string sourceDirPath, string destDirPath)
    {
        Directory.CreateDirectory(destDirPath);

        var dir = new DirectoryInfo(sourceDirPath);
        foreach (var file in dir.EnumerateFiles())
        {
            var dest = Path.Combine(destDirPath, file.Name);
            _console.WriteLine($"copying {file.FullName} into {dest}");
            file.CopyTo(dest, overwrite: true);
        }

        foreach (var subdir in dir.EnumerateDirectories())
        {
            CopyEntireDirectory(subdir.FullName, Path.Combine(destDirPath, subdir.Name));
        }
    }

    /// <summary>
    /// Create a git mirror clone of a repository. If the clone already exists, the operation is skipped.
    /// </summary>
    /// <param name="owner">The owner of the repository to be cloned.</param>
    /// <param name="repository">The repository name to be cloned.</param>
    /// <param name="options">The BackupOptions.</param>
    private void CreateClone(string owner, string repository, BackupOptions options)
    {
        var dest = Path.Combine(options.DestinationRepositoriesDir, owner, repository);
        if (Directory.Exists(dest))
        {
            _console.WriteLine($"{dest} already exists, skipping git clone operation");
            return;
        }

        var source = Path.Combine(options.GitBucketHomeRepositoriesDir, owner, repository);
        _console.WriteLine($"cloning {source} into {dest}");

        LibGit2Sharp.Repository.Clone(source, dest, new CloneOptions { IsBare = true });
    }

    /// <summary>
    /// Check if pg_dump is available.
    /// </summary>
    /// <param name="options">The BackupOptions.</param>
    /// <returns>true if available, false if not.</returns>
    private bool PgDumpAvailable(BackupOptions options)
    {
        _console.WriteLine("Checking pg_dump existence...");

        using var process = new Process();
        process.StartInfo.FileName = string.IsNullOrEmpty(options.PgDump) ? "pg_dump" : options.PgDump;
        process.StartInfo.Arguments = "--help";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;

        try
        {
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0 || !output.Contains("pg_dump [OPTION]... [DBNAME]", StringComparison.OrdinalIgnoreCase))
            {
                _console.WriteWarnLine($"Cannot found valid pg_dump.");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _console.WriteWarnLine(ex.ToString());
            return false;
        }
    }

    /// <summary>
    /// Export the GitBucket database as a plain text dump using pg_dump.
    /// </summary>
    /// <param name="options">The BackupOptions.</param>
    /// <param name="connectionString">The connectionString.</param>
    private void DumpDatabase(BackupOptions options, string connectionString)
    {
        _console.WriteLine("Database backup");

        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var outputFile = Path.Combine(options.Destination, $"{builder.Database}_{DateTime.Now:yyyyMMddHHmmss}.sql");

        using var process = new Process();
        process.StartInfo.FileName = string.IsNullOrEmpty(options.PgDump) ? "pg_dump" : options.PgDump;
        process.StartInfo.Arguments = $"--host={builder.Host} --port={builder.Port} --username={builder.Username} --dbname={builder.Database} --file={outputFile} --format=plain";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.Environment.Add("PGPASSWORD", builder.Password);

        try
        {
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            _console.WriteLine($"The database dump is available in {outputFile}");
        }
        catch (Exception ex)
        {
            _console.WriteWarnLine(ex.ToString());
        }
    }
}
