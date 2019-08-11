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
        public Issue SourceIssue1 { get; set; } = default!;
        public Issue SourceIssue2 { get; set; } = default!;

        [Fact]
        public async void Should_Copy_an_Issue_to_a_Different_Repository()
        {
            // Arange
            await InitializeAsync();

            // Act
            var output = Execute($"issue -t copy -s {Repository1.FullName} -d {Repository2.FullName} -n {SourceIssue1.Number}");

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
        public async void Should_Copy_Multiple_Issues_to_a_Different_Repository()
        {
            // Arange
            await InitializeAsync(createMultipleIssues: true);

            // Act
            var output = Execute($"issue -t copy -s {Repository1.FullName} -d {Repository2.FullName} -n 1:2");

            // Assert
            Assert.Equal(@$"The issue has been successfully copied to http://localhost:8080/{Repository2.FullName}/issues/1 .
The issue has been successfully copied to http://localhost:8080/{Repository2.FullName}/issues/2 .
", output);

            var newIssues = await GitBucketFixture.GitBucketClient.Issue.GetAllForRepository(GitBucketDefaults.Owner, Repository2.Name);
            Assert.Equal(2, newIssues.Count);
            Assert.Equal("Second Issue title", newIssues[0].Title);
            Assert.Equal(@$"Second issue content.

*Copied from original issue: {Repository1.FullName}#2*",
                newIssues[0].Body);
            Assert.Equal("First Issue title", newIssues[1].Title);
            Assert.Equal(@$"First issue content.

*Copied from original issue: {Repository1.FullName}#1*",
                newIssues[1].Body);
        }

        [Fact]
        public async void Should_Move_an_Issue_to_a_Different_Repository()
        {
            // Arange
            await InitializeAsync();
            var createdAt = SourceIssue1.CreatedAt.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            // Act
            var output = Execute($"issue -t move -s {Repository1.FullName} -d {Repository2.FullName} -n {SourceIssue1.Number}");

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

        [Fact]
        public async void Should_Move_Multiple_Issues_to_a_Different_Repository()
        {
            // Arange
            await InitializeAsync(createMultipleIssues: true);
            var createdAt = SourceIssue1.CreatedAt.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            // Act
            var output = Execute($"issue -t move -s {Repository1.FullName} -d {Repository2.FullName} -n 1:2");

            // Assert
            var newIssues = await GitBucketFixture.GitBucketClient.Issue.GetAllForRepository(GitBucketDefaults.Owner, Repository2.Name);
            Assert.Equal(2, newIssues.Count);
            Assert.Equal(@$"The issue has been successfully moved to http://localhost:8080/{Repository2.FullName}/issues/1 .
Close the original one manually.
The issue has been successfully moved to http://localhost:8080/{Repository2.FullName}/issues/2 .
Close the original one manually.
", output);
            Assert.Equal("Second Issue title", newIssues[0].Title);
            Assert.Equal(@$"*From @root on {createdAt}*

Second issue content.

*Copied from original issue: {Repository1.FullName}#2*", newIssues[0].Body);

            Assert.Equal("First Issue title", newIssues[1].Title);
            Assert.Equal(@$"*From @root on {createdAt}*

First issue content.

*Copied from original issue: {Repository1.FullName}#1*", newIssues[1].Body);

            var sourceComments1 = await GitBucketFixture.GitBucketClient.Issue.Comment.GetAllForIssue(GitBucketDefaults.Owner, Repository1.Name, 1);
            Assert.Single(sourceComments1);
            Assert.Equal($"*This issue was moved to {Repository2.FullName}#1*", sourceComments1[0].Body);

            var sourceComments2 = await GitBucketFixture.GitBucketClient.Issue.Comment.GetAllForIssue(GitBucketDefaults.Owner, Repository1.Name, 2);
            Assert.Single(sourceComments2);
            Assert.Equal($"*This issue was moved to {Repository2.FullName}#2*", sourceComments2[0].Body);
        }

        private async Task InitializeAsync(bool createMultipleIssues = false)
        {
            Repository1 = await CreateRepository(autoInit: true);
            Repository2 = await CreateRepository(autoInit: true);

            SourceIssue1 = await GitBucketFixture.GitBucketClient.Issue.Create(
                GitBucketDefaults.Owner, Repository1.Name, new NewIssue("First Issue title")
                {
                    Body = "First issue content."
                });

            if (createMultipleIssues)
            {
                SourceIssue2 = await GitBucketFixture.GitBucketClient.Issue.Create(
                    GitBucketDefaults.Owner, Repository1.Name, new NewIssue("Second Issue title")
                    {
                        Body = "Second issue content."
                    });
            }
        }
    }
}
