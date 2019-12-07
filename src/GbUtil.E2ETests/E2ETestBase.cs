using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Octokit;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
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

        protected static string Execute(string arguments)
        {
            using var process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $@"""{GbUtilDll}"" {arguments}";
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

        protected void SetMilestone(Issue issue)
        {
            if (issue is null)
            {
                throw new ArgumentNullException(nameof(issue));
            }

            GitBucketFixture.Driver.Navigate().GoToUrl(new Uri($"{GitBucketDefaults.BaseUri}{Repository.FullName}/issues/{issue.Number}"));
            var wait = new WebDriverWait(GitBucketFixture.Driver, new TimeSpan(0, 0, 15));
            wait.Until(drv => drv.Title == $"{issue.Title} - Issue #{issue.Number} - {Repository.FullName}");

            // GitBucket issue page has multiple elements whose id is "test".
            // Milestone dropdown is the fourth of them.
            var tests = GitBucketFixture.Driver.FindElements(By.XPath(@"//*[@id=""test""]"));
            tests[3].Click();
            var dropdownMenus = GitBucketFixture.Driver.FindElements(By.XPath("//ul[@class='dropdown-menu pull-right']//li/a"));
            foreach (var dropdownMenu in dropdownMenus)
            {
                var dataTitle = dropdownMenu.GetAttribute("data-title");
                if (!string.IsNullOrEmpty(dataTitle))
                {
                    if (dataTitle.Contains("v1.0.0", StringComparison.OrdinalIgnoreCase))
                    {
                        dropdownMenu.Click();
                        break;
                    }
                }
            }
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
