﻿using System;
using System.Threading.Tasks;
using LibGit2Sharp;
using Octokit;
using OpenQA.Selenium;
using Xunit;

namespace GbUtil.E2ETests
{
    public class PullRequestTest : E2ETestBase
    {
        public PullRequestTest(GitBucketFixture fixture) : base(fixture)
        {
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
        public async Task Should_Create_Draft_PullRequest()
        {
            // Arrange
            await PrepareForPR();

            // Act
            _ = Execute($"release -o {GitBucketDefaults.Owner} -r {Repository.Name} -m v1.0.0 --create-pr --draft -f");

            // Assert
            GitBucketFixture.Driver.Navigate().GoToUrl(new Uri($"{GitBucketDefaults.BaseUri}{Repository.FullName}/pull/3"));
            var mergeButton = GitBucketFixture.Driver.FindElement(By.Id("merge-pull-request-button"));
            Assert.False(mergeButton.Enabled);
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

        private async Task PrepareForPR()
        {
            // Create base and target branch
            Repository = await CreateRepository(autoInit: true);
            CreateBranch("develop");
            await UpdateReadme("develop");

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
    }
}
