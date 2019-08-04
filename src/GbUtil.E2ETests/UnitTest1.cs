using System;
using System.Linq;
using Octokit;
using Xunit;

namespace GbUtil.E2ETests
{
    public class UnitTest1 : IClassFixture<GitBucketFixture>
    {
        private readonly GitBucketFixture _fixture;

        public UnitTest1(GitBucketFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void Should_Copy_issues()
        {
            await _fixture.GitBucketClient.Repository.Create(new NewRepository(GitBucketDefaults.Repository1) { AutoInit = true });
            await _fixture.GitBucketClient.Repository.Create(new NewRepository(GitBucketDefaults.Repository2) { AutoInit = true });

            var sourceIssue = await _fixture.GitBucketClient.Issue.Create(GitBucketDefaults.Owner, GitBucketDefaults.Repository1, new NewIssue("First Issue title")
            {
                Body = "First issue content."
            });

            var result = GitBucketFixture.Execute($"issue -t copy -s {GitBucketDefaults.Owner}/{GitBucketDefaults.Repository1} -d {GitBucketDefaults.Owner}/{GitBucketDefaults.Repository2} -n {sourceIssue.Number}");

            // Assert
            Assert.Equal($"The issue has been successfully copied to http://localhost:8080/root/{GitBucketDefaults.Repository2}/issues/1 .{Environment.NewLine}", result);
            var newIssue = await _fixture.GitBucketClient.Issue.Get(GitBucketDefaults.Owner, GitBucketDefaults.Repository1, 1);
        }
    }
}
