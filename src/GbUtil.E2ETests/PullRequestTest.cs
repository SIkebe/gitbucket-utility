using System;
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
    public class PullRequestTest : E2ETestBase, IDisposable
    {
        private bool disposedValue = false;

        public PullRequestTest(GitBucketFixture fixture) : base(fixture)
        {
            Credentials = new UsernamePasswordCredentials { Username = GitBucketDefaults.Owner, Password = GitBucketDefaults.Password };
            WorkingDir = Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(E2ETestBase))!.Location)!, Guid.NewGuid().ToString());
        }

        public UsernamePasswordCredentials Credentials { get; }
        public Octokit.Repository Repository { get; set; } = default!;
        public string WorkingDir { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task Should_Create_PullRequest()
        {
            // Arrange
            await PrepareForPR();

            // Act
            var output1 = Execute($"release -o {GitBucketDefaults.Owner} -r {Repository.Name} -m v1.0.0 --create-pr -f");
            var output2 = Execute($"release -o {GitBucketDefaults.Owner} -r {Repository.Name} -m v1.0.0 --create-pr -f");

            // Assert
            Assert.Equal($"A new pull request has been successfully created!{Environment.NewLine}", output1);
            Assert.Equal($"A pull request already exists for {GitBucketDefaults.Owner}:develop.{Environment.NewLine}", output2);

            var pr = await GitBucketFixture.GitBucketClient.PullRequest.Get(GitBucketDefaults.Owner, Repository.Name, 3);
            Assert.Equal("master", pr.Base.Ref);
            Assert.Equal("develop", pr.Head.Ref);
            Assert.Equal("v1.0.0", pr.Title);
            Assert.Equal(ItemState.Open, pr.State);
            Assert.Equal(@"As part of this release we had 2 issues closed.
The highest priority among them is """".

### Enhancement
* Bump to v1.0.0 #1

### Bug
* Found a bug #2

", pr.Body);
        }

        [Fact]
        public async Task Should_Output_ReleaseNote()
        {
            // Arrange
            await PrepareForPR();

            // Act
            var output = Execute($"release -o {GitBucketDefaults.Owner} -r {Repository.Name} -m v1.0.0 -f");

            // Assert
            Assert.Equal(@"As part of this release we had 2 issues closed.
The highest priority among them is """".

### Enhancement
* Bump to v1.0.0 #1

### Bug
* Found a bug #2


", output);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    var directoryInfo = new DirectoryInfo(WorkingDir);
                    RemoveReadonlyAttribute(directoryInfo);
                    directoryInfo.Delete(recursive: true);
                }

                disposedValue = true;
            }
        }

        private async Task PrepareForPR()
        {
            // Create base and target branch
            Repository = await CreateRepository(autoInit: true);
            CreateDevelopBranch(Credentials);
            await UpdateReadme();

            // Create issues which target milestone v1.0.0
            var issue1 = await GitBucketFixture.GitBucketClient.Issue.Create(GitBucketDefaults.Owner, Repository.Name, new NewIssue("Bump to v1.0.0"));
            await GitBucketFixture.GitBucketClient.Issue.Labels.AddToIssue(GitBucketDefaults.Owner, Repository.Name, issue1.Number, new[] { "Enhancement" });

            var issue2 = await GitBucketFixture.GitBucketClient.Issue.Create(GitBucketDefaults.Owner, Repository.Name, new NewIssue("Found a bug"));
            await GitBucketFixture.GitBucketClient.Issue.Labels.AddToIssue(GitBucketDefaults.Owner, Repository.Name, issue2.Number, new[] { "Bug" });

            // Create milestone v1.0.0 and set above issues to it
            GitBucketFixture.CreateMilestone(Repository, "v1.0.0");
            SetMilestone(issue1);
            SetMilestone(issue2);
        }

        private async Task UpdateReadme()
        {
            var contents = await GitBucketFixture.GitBucketClient.Repository.Content.GetAllContentsByRef(GitBucketDefaults.Owner, Repository.Name, "develop");
            var readme = contents.Where(c => c.Name == "README.md").Single();
            await GitBucketFixture.GitBucketClient.Repository.Content.UpdateFile(
                GitBucketDefaults.Owner,
                Repository.Name,
                "README.md",
                new UpdateFileRequest("New commit message.", "New file content.", readme.Sha, "develop"));
        }

        private void SetMilestone(Issue issue)
        {
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

        private void CreateDevelopBranch(UsernamePasswordCredentials credentials)
        {
            // git clone http://localhost:8080/git/root/<repository-name>.git
            LibGit2Sharp.Repository.Clone($"{GitBucketDefaults.BaseUri}git/{Repository.FullName}.git", WorkingDir, new CloneOptions
            {
                CredentialsProvider = (url, user, cred) => credentials
            });

            // TODO: Use Web API to create a new branch if implemented in GitBucket
            using var repo = new LibGit2Sharp.Repository(WorkingDir);

            // git checkout -b develop
            repo.CreateBranch("develop");
            var localBranch = Commands.Checkout(repo, "develop");

            // git branch -u origin/develop
            Remote remote = repo.Network.Remotes["origin"];
            repo.Branches.Update(localBranch, b => b.Remote = remote.Name, b => b.UpstreamBranch = localBranch.CanonicalName);

            // git push origin develop
            repo.Network.Push(repo.Branches["develop"], new PushOptions
            {
                CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) => credentials)
            });
        }
    }
}
