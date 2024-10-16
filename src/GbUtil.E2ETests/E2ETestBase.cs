using System.Diagnostics;
using System.Reflection;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Octokit;
using Xunit;
using Xunit.Abstractions;

namespace GbUtil.E2ETests;

public abstract class E2ETestBase : IClassFixture<GitBucketFixture>, IDisposable
{
    private bool _disposedValue;

    protected E2ETestBase(GitBucketFixture fixture, ITestOutputHelper output)
    {
        GitBucketFixture = fixture;
        Output = output;
        Credentials = new UsernamePasswordCredentials { Username = GitBucketDefaults.Owner, Password = GitBucketDefaults.Password };
        WorkingDir = Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(E2ETestBase))!.Location)!, Guid.NewGuid().ToString());
    }

    public static string GbUtilDll
    {
        get
        {
            var gbUtilE2ETestsDll = Assembly.GetAssembly(typeof(E2ETestBase))!.Location;
            var gbUtilDll = gbUtilE2ETestsDll.Replace(".E2ETests", string.Empty, StringComparison.OrdinalIgnoreCase);
            return gbUtilDll;
        }
    }

    public UsernamePasswordCredentials Credentials { get; }
    public Octokit.Repository Repository { get; set; } = default!;
    public string WorkingDir { get; }

    public GitBucketFixture GitBucketFixture { get; set; }
    public ITestOutputHelper Output { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<Octokit.Repository> CreateRepository(bool autoInit = false)
    {
        var repository = Guid.NewGuid().ToString();
        return await GitBucketFixture.GitBucketClient.Repository.Create(new NewRepository(repository) { AutoInit = autoInit });
    }

    protected static string Execute(string arguments)
    {
        using var process = new Process();
        var useSingleFileExe = Environment.GetEnvironmentVariable("GbUtil_UseSingleFileExe");
        var singleFileExePath = Environment.GetEnvironmentVariable("GbUtil_SingleFileExePath");
        if (useSingleFileExe == "true" && singleFileExePath is not null && File.Exists(singleFileExePath))
        {
            process.StartInfo.FileName = singleFileExePath;
            process.StartInfo.Arguments = $@"{arguments}";
        }
        else
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $@"""{GbUtilDll}"" {arguments}";
        }

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;

        process.Start();

        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output;
    }

    protected static void RemoveReadonlyAttribute(DirectoryInfo directoryInfo)
    {
        ArgumentNullException.ThrowIfNull(directoryInfo);

        if ((directoryInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
        {
            directoryInfo.Attributes = FileAttributes.Normal;
        }

        foreach (var fi in directoryInfo.GetFiles())
        {
            if ((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                fi.Attributes = FileAttributes.Normal;
            }
        }

        foreach (var di in directoryInfo.GetDirectories())
        {
            RemoveReadonlyAttribute(di);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                if (Directory.Exists(WorkingDir))
                {
                    var directoryInfo = new DirectoryInfo(WorkingDir);
                    RemoveReadonlyAttribute(directoryInfo);
                    directoryInfo.Delete(recursive: true);
                }
            }

            _disposedValue = true;
        }
    }

    protected async Task UpdateReadme(string branchName)
    {
        var contents = await GitBucketFixture.GitBucketClient.Repository.Content.GetAllContentsByRef(GitBucketDefaults.Owner, Repository.Name, branchName);
        var readme = contents.Where(c => c.Name == "README.md").Single();
        await GitBucketFixture.GitBucketClient.Repository.Content.UpdateFile(
            GitBucketDefaults.Owner,
            Repository.Name,
            "README.md",
            new UpdateFileRequest("New commit message.", "New file content.", readme.Sha, branchName));
    }

    protected void CreateBranch(string branchName)
    {
        if (!Directory.Exists(WorkingDir))
        {
            var options = new CloneOptions();
            options.FetchOptions.CredentialsProvider = (url, user, cred) => Credentials;

            // git clone http://localhost:8080/git/root/<repository-name>.git
            LibGit2Sharp.Repository.Clone($"{GitBucketDefaults.BaseUri}git/{Repository.FullName}.git", WorkingDir, options);
        }

        // TODO: Use Web API to create a new branch if implemented in GitBucket
        using var repo = new LibGit2Sharp.Repository(WorkingDir);

        // git checkout -b <new-branch>
        repo.CreateBranch(branchName);
        var localBranch = Commands.Checkout(repo, branchName);

        // git branch -u origin/<new-branch>
        var remote = repo.Network.Remotes["origin"];
        repo.Branches.Update(localBranch, b => b.Remote = remote.Name, b => b.UpstreamBranch = localBranch.CanonicalName);

        // git push origin <new-branch>
        repo.Network.Push(repo.Branches[branchName], new PushOptions
        {
            CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) => Credentials)
        });
    }
}
