using System;
using System.IO;
using System.Linq;
using GitBucket.Core;
using LibGit2Sharp;

namespace GitBucket.Service
{
    public interface IBackupService
    {
        int Backup(BackupOptions options);
    }

    /// <summary>
    /// Port of https://github.com/gitbucket/gitbucket/wiki/Backup for C# & PostgreSQL.
    /// </summary>
    public class BackupService : IBackupService
    {
        private readonly IConsole _console;

        public BackupService(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        /// <summary>
        /// To keep integrity as its maximum possible, the database export and git backups must be done in the shortest possible timeslot.
        /// Thus we will:
        ///  - clone new repositories into backup directory
        ///  - update them all a first time, so that we gather updates
        ///  - export the database
        ///  - update all repositories a second time, this time it should be ultra-fast
        /// </summary>
        /// <param name="options">The BackupOptions.</param>
        /// <returns>Return code.</returns>
        public int Backup(BackupOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Directory.CreateDirectory(options.Destination);
            Directory.CreateDirectory(options.DestinationRepositoriesDir);

            CloneRepositories(options);

            _console.WriteLine("Update repositories: phase 1");
            UpdateRepositories(options);
            _console.WriteLine("Update repositories: phase 1, terminated");

            // Export the GitBucket configuration
            _console.WriteLine("Configuration backup");
            File.Copy(options.GitBucketHomeConfigurationFile, options.DestinationConfigurationFile, overwrite: true);

            // Export the GitBucket data directory (avatars, ...)
            if (Directory.Exists(options.GitBucketHomeDataDir))
            {
                _console.WriteLine("Avatars backup");
                CopyEntireDirectory(options.GitBucketHomeDataDir, options.DestinationDataDir);
            }

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
            foreach (var oweners in Directory.EnumerateDirectories(options.DestinationRepositoriesDir))
            {
                foreach (var repositoryPath in Directory.EnumerateDirectories(oweners, "*.git", SearchOption.TopDirectoryOnly))
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
        /// Clone all repositories in repositoryBackupFolder.
        /// </summary>
        /// <param name="options">The BackupOptions.</param>
        private void CloneRepositories(BackupOptions options)
        {
            _console.WriteLine("Starting clone process");

            foreach (var ownerPath in Directory.EnumerateDirectories(options.GitBucketHomeRepositoriesDir, "*", SearchOption.TopDirectoryOnly))
            {
                var owner = new DirectoryInfo(ownerPath).Name;
                Directory.CreateDirectory(Path.Combine(options.DestinationRepositoriesDir, owner));

                var repositories = Directory.EnumerateDirectories(ownerPath, "*.git", SearchOption.TopDirectoryOnly);
                foreach (var repositoryPath in repositories)
                {
                    var repository = new DirectoryInfo(repositoryPath).Name;
                    CreateClone(owner, repository, options);
                }
            }

            _console.WriteLine("All repositories, cloned");
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
    }
}
