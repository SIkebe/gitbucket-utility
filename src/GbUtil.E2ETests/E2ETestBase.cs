using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GitBucket.Core;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Octokit;
using Xunit;

namespace GbUtil.E2ETests
{
    public abstract class E2ETestBase : IClassFixture<GitBucketFixture>, IDisposable
    {
        private bool disposedValue = false;

        public E2ETestBase(GitBucketFixture fixture)
        {
            GitBucketFixture = fixture;
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

        protected static void CreateMilestone(string owner, string repository, string title, string? description = null, DateTime? dueDate = null)
        {
            using var dbContext = new GitBucketDbContext(GitBucketDefaults.ConnectionStrings);
            dbContext.Milestone.Add(new GitBucket.Core.Models.Milestone
            {
                UserName = owner,
                RepositoryName = repository,
                Title = title,
                Description = description,
                DueDate = dueDate,
            });

            dbContext.SaveChanges();
        }

        protected static string Execute(string arguments)
        {
            using var process = new Process();
            var useSingleFileExe = Environment.GetEnvironmentVariable("GbUtil_UseSingleFileExe");
            var singleFileExePath = Environment.GetEnvironmentVariable("GbUtil_SingleFileExePath");
            if (useSingleFileExe == "true" && !(singleFileExePath is null) && File.Exists(singleFileExePath))
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
            if (directoryInfo is null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

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

        protected static void SetMilestone(string owner, string repository, int issueNumber, string milestoneTitle)
        {
            using var dbContext = new GitBucketDbContext(GitBucketDefaults.ConnectionStrings);
            var milestone = dbContext.Milestone
                .Where(m => m.UserName == owner)
                .Where(m => m.RepositoryName == repository)
                .Where(m => m.Title == milestoneTitle)
                .Single();

            var target = dbContext.Issue
                .Where(i => i.UserName == owner)
                .Where(i => i.RepositoryName == repository)
                .Where(i => i.IssueId == issueNumber)
                .Single();

            target.Milestone = milestone;
            dbContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
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

                disposedValue = true;
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
                // git clone http://localhost:8080/git/root/<repository-name>.git
                LibGit2Sharp.Repository.Clone($"{GitBucketDefaults.BaseUri}git/{Repository.FullName}.git", WorkingDir, new CloneOptions
                {
                    CredentialsProvider = (url, user, cred) => Credentials
                });
            }

            // TODO: Use Web API to create a new branch if implemented in GitBucket
            using var repo = new LibGit2Sharp.Repository(WorkingDir);

            // git checkout -b <new-branch>
            repo.CreateBranch(branchName);
            var localBranch = Commands.Checkout(repo, branchName);

            // git branch -u origin/<new-branch>
            Remote remote = repo.Network.Remotes["origin"];
            repo.Branches.Update(localBranch, b => b.Remote = remote.Name, b => b.UpstreamBranch = localBranch.CanonicalName);

            // git push origin <new-branch>
            repo.Network.Push(repo.Branches[branchName], new PushOptions
            {
                CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) => Credentials)
            });
        }
    }
}
