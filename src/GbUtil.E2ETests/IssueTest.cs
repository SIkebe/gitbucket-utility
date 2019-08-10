using System;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using Xunit;

namespace GbUtil.E2ETests
{
    public class IssueTest : E2ETestBase
    {
        public IssueTest(GitBucketFixture fixture) : base(fixture)
        {
        }

        public Repository Repository1 { get; set; } = default!;
        public Repository Repository2 { get; set; } = default!;
        public Issue SourceIssue { get; set; } = default!;

        [Fact]
        public async void Should_Copy_Issue_To_Different_Repository()
        {
            // Arange
            await InitializeAsync();

            // Act
            var output = Execute($"issue -t copy -s {Repository1.FullName} -d {Repository2.FullName} -n {SourceIssue.Number}");

            // Assert
            Assert.Equal(@$"The issue has been successfully copied to http://localhost:8080/{Repository2.FullName}/issues/1 .
", output);

            var newIssue = await GitBucketFixture.GitBucketClient.Issue.Get(GitBucketDefaults.Owner, Repository2.Name, 1);
            Assert.Equal("First Issue title", newIssue.Title);
            Assert.Equal(@$"First issue content.

*Copied from original issue: {Repository1.FullName}#1*",
                newIssue.Body);
        }

        [Fact]
        public async void Should_Move_Issue_To_Different_Repository()
        {
            // Arange
            await InitializeAsync();
            var createdAt = SourceIssue.CreatedAt.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            // Act
            var output = Execute($"issue -t move -s {Repository1.FullName} -d {Repository2.FullName} -n {SourceIssue.Number}");

            // Assert
            var newIssue = await GitBucketFixture.GitBucketClient.Issue.Get(GitBucketDefaults.Owner, Repository2.Name, 1);
            Assert.Equal(@$"The issue has been successfully moved to http://localhost:8080/{Repository2.FullName}/issues/1 .
Close the original one manually.
", output);
            Assert.Equal("First Issue title", newIssue.Title);
            Assert.Equal(@$"*From @root on {createdAt}*

First issue content.

*Copied from original issue: {Repository1.FullName}#1*", newIssue.Body);

            var sourceComments = await GitBucketFixture.GitBucketClient.Issue.Comment.GetAllForIssue(GitBucketDefaults.Owner, Repository1.Name, 1);
            Assert.Single(sourceComments);
            Assert.Equal($"*This issue was moved to {Repository2.FullName}#{newIssue.Number}*", sourceComments[0].Body);
        }

        private async Task InitializeAsync()
        {
            Repository1 = await CreateRepository(autoInit: true);
            Repository2 = await CreateRepository(autoInit: true);

            SourceIssue = await GitBucketFixture.GitBucketClient.Issue.Create(
                GitBucketDefaults.Owner, Repository1.Name, new NewIssue("First Issue title")
                {
                    Body = "First issue content."
                });
        }
    }
}
