using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GitBucket.Core;
using Moq;
using Octokit;
using Xunit;

namespace GitBucket.Service.Tests
{
    public class IssueServiceTest
    {
        private readonly User _rootUser = new User
        (
            avatarUrl: "",
            bio: "",
            blog: "",
            collaborators: 0,
            company: "",
            createdAt: new DateTimeOffset(new DateTime(2018, 7, 1)),
            updatedAt: new DateTimeOffset(new DateTime(2018, 7, 1)),
            diskUsage: 0,
            email: "",
            followers: 0,
            following: 0,
            hireable: false,
            htmlUrl: "",
            totalPrivateRepos: 0,
            id: 0,
            location: "",
            login: "root",
            name: "root",
            nodeId: "",
            ownedPrivateRepos: 0,
            plan: null,
            privateGists: 0,
            publicGists: 0,
            publicRepos: 0,
            url: "",
            permissions: null,
            siteAdmin: true,
            ldapDistinguishedName: "",
            suspendedAt: null
        );

        private readonly User _user1 = new User
        (
            avatarUrl: "",
            bio: "",
            blog: "",
            collaborators: 0,
            company: "",
            createdAt: new DateTimeOffset(new DateTime(2018, 8, 1)),
            updatedAt: new DateTimeOffset(new DateTime(2018, 8, 2)),
            diskUsage: 0,
            email: "",
            followers: 0,
            following: 0,
            hireable: false,
            htmlUrl: "",
            totalPrivateRepos: 0,
            id: 1,
            location: "",
            login: "user1",
            name: "user1",
            nodeId: "",
            ownedPrivateRepos: 0,
            plan: null,
            privateGists: 0,
            publicGists: 0,
            publicRepos: 0,
            url: "",
            permissions: null,
            siteAdmin: true,
            ldapDistinguishedName: "",
            suspendedAt: null
        );

        [Fact]
        public async Task Should_Move_Issue_To_Same_Owner_Repository()
        {
            // Given
            var mockGitBucketClient = new Mock<IGitHubClient>(MockBehavior.Strict);
            mockGitBucketClient
                .Setup(g => g.Issue.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((string owner, string repository, int id) => new Octokit.Issue
                (
                    url: "",
                    htmlUrl: "",
                    commentsUrl: "",
                    eventsUrl: "",
                    number: 0,
                    state: ItemState.Open,
                    title: "Found a bug",
                    body: "Original Issue Comment.",
                    closedBy: null,
                    user: _rootUser,
                    labels: null,
                    assignee: new User(),
                    assignees: null,
                    milestone: null,
                    comments: 0,
                    pullRequest: null,
                    closedAt: null,
                    createdAt: new DateTimeOffset(new DateTime(2018, 7, 1)),
                    updatedAt: new DateTimeOffset(new DateTime(2018, 7, 1)),
                    id: id,
                    nodeId: "",
                    locked: false,
                    repository: null,
                    reactions: null
                ));

            mockGitBucketClient
                .Setup(g => g.Issue.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NewIssue>()))
                .ReturnsAsync((string owner, string repository, NewIssue newIssue) => new Octokit.Issue
                (
                    url: "",
                    htmlUrl: "",
                    commentsUrl: "",
                    eventsUrl: "",
                    number: 1,
                    state: ItemState.Open,
                    title: newIssue.Title,
                    body: newIssue.Body,
                    closedBy: null,
                    user: new User(),
                    labels: null,
                    assignee: new User(),
                    assignees: null,
                    milestone: null,
                    comments: 0,
                    pullRequest: null,
                    closedAt: null,
                    createdAt: new DateTimeOffset(new DateTime(2018, 7, 2)),
                    updatedAt: new DateTimeOffset(new DateTime(2018, 7, 2)),
                    id: 1,
                    nodeId: "",
                    locked: false,
                    repository: null,
                    reactions: null
                ));

            mockGitBucketClient
                .Setup(g => g.Issue.Comment.GetAllForIssue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.IssueComment>(new List<Octokit.IssueComment>{
                    new Octokit.IssueComment(
                        id:1,
                        nodeId:"",
                        url:"",
                        htmlUrl:"",
                        body:"This is a comment by root.",
                        createdAt:new DateTimeOffset(new DateTime(2018, 1, 1)),
                        updatedAt:new DateTimeOffset(new DateTime(2018, 1, 2)),
                        user:_user1,
                        reactions:new ReactionSummary()
                    )
                }));

            mockGitBucketClient
                .Setup(g => g.Issue.Comment.Create(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new Octokit.IssueComment());

            var options = new IssueOptions
            {
                ExecutedDate = new DateTime(2018, 7, 1),
                Source = "root/test1",
                Destination = "root/test2",
                IssueNumber = 1
            };

            var console = new FakeConsole();
            var service = new IssueService(console);

            // When
            var result = await service.Execute(options, mockGitBucketClient.Object);

            // Then
            Assert.Equal(0, result);
            mockGitBucketClient.Verify(g => g.Issue.Get(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test1"),
                It.Is<int>(i => i == 1)));

            mockGitBucketClient.Verify(g => g.Issue.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<NewIssue>(i =>
                    i.Title == "Found a bug" &&
                    i.Body == "*From @root on 2018-07-01 00:00:00*\r\n\r\nOriginal Issue Comment.\r\n\r\n*Copied from original issue: root/test1#1*")));

            mockGitBucketClient.Verify(g => g.Issue.Comment.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(n => n == 1),
                It.Is<string>(c => c == "*From @user1 on 2018-01-01 00:00:00*\r\n\r\nThis is a comment by root.")));

            mockGitBucketClient.Verify(g => g.Issue.Comment.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test1"),
                It.Is<int>(n => n == 0),
                It.Is<string>(c => c == "*This issue was moved to root/test2#1*")));
        }

        [Fact]
        public async Task Unsupported_Issue_Type()
        {
            // Given
            var mockGitBucketClient = new Mock<IGitHubClient>(MockBehavior.Strict);
            var options = new IssueOptions
            {
                ExecutedDate = new DateTime(2018, 7, 1),
                Source = "root/test1",
                Destination = "root/test2",
                IssueNumber = 1,
                Type = "Invalid Type"
            };

            var console = new FakeConsole();
            var service = new IssueService(console);

            // When
            var result = await service.Execute(options, mockGitBucketClient.Object);

            // Then
            Assert.Equal(1, result);
            Assert.Single(console.WarnMessages);
            Assert.Equal(@"""Invalid Type"" is not supported.", console.WarnMessages.First());
        }

        [Theory]
        [InlineData("roottest1")]
        [InlineData("/roottest1")]
        [InlineData("roottest1/")]
        [InlineData("root/tes/t1")]
        public async Task Invalid_Source_Format(string source)
        {
            // Given
            var mockGitBucketClient = new Mock<IGitHubClient>(MockBehavior.Strict);
            var options = new IssueOptions
            {
                ExecutedDate = new DateTime(2018, 7, 1),
                Source = source,
                Destination = "root/test2",
                IssueNumber = 1
            };

            var console = new FakeConsole();
            var service = new IssueService(console);

            // When
            var result = await service.Execute(options, mockGitBucketClient.Object);

            // Then
            Assert.Equal(1, result);
            Assert.Single(console.WarnMessages);
            Assert.Equal("Incorrect source format.", console.WarnMessages.First());
        }

        [Theory]
        [InlineData("roottest2")]
        [InlineData("/roottest2")]
        [InlineData("roottest2/")]
        [InlineData("root/tes/t2")]
        public async Task Invalid_Destination_Format(string destination)
        {
            // Given
            var mockGitBucketClient = new Mock<IGitHubClient>(MockBehavior.Strict);
            var options = new IssueOptions
            {
                ExecutedDate = new DateTime(2018, 7, 1),
                Source = "root/test1",
                Destination = destination,
                IssueNumber = 1
            };

            var console = new FakeConsole();
            var service = new IssueService(console);

            // When
            var result = await service.Execute(options, mockGitBucketClient.Object);

            // Then
            Assert.Equal(1, result);
            Assert.Single(console.WarnMessages);
            Assert.Equal("Incorrect destination format.", console.WarnMessages.First());
        }

        [Fact]
        public async Task Should_Throw_If_IssueOptions_Is_Null()
        {
            // Given
            var mockGitBucketClient = new Mock<IGitHubClient>(MockBehavior.Strict);

            var console = new FakeConsole();
            var service = new IssueService(console);

            // When
            var ex = await Record.ExceptionAsync(() => service.Execute(null, mockGitBucketClient.Object));

            // Then
            Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal("Value cannot be null.\r\nParameter name: options", ex.Message);
        }

        [Fact]
        public async Task Should_Throw_If_Client_Is_Null()
        {
            // Given
            var options = new IssueOptions
            {
                ExecutedDate = new DateTime(2018, 7, 1),
                Source = "root/test1",
                Destination = "root/test2",
                IssueNumber = 1
            };

            var console = new FakeConsole();
            var service = new IssueService(console);

            // When
            var ex = await Record.ExceptionAsync(() => service.Execute(options, null));

            // Then
            Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal("Value cannot be null.\r\nParameter name: gitBucketClient", ex.Message);
        }

        [Fact]
        public async Task Should_Copy_Issue_To_Same_Owner_Repository()
        {
            // Given
            var mockGitBucketClient = new Mock<IGitHubClient>(MockBehavior.Strict);
            mockGitBucketClient
                .Setup(g => g.Issue.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((string owner, string repository, int id) => new Octokit.Issue
                (
                    url: "",
                    htmlUrl: "",
                    commentsUrl: "",
                    eventsUrl: "",
                    number: 0,
                    state: ItemState.Open,
                    title: "Found a bug",
                    body: "Original Issue Comment.",
                    closedBy: null,
                    user: _rootUser,
                    labels: null,
                    assignee: new User(),
                    assignees: null,
                    milestone: null,
                    comments: 0,
                    pullRequest: null,
                    closedAt: null,
                    createdAt: new DateTimeOffset(new DateTime(2018, 7, 1)),
                    updatedAt: new DateTimeOffset(new DateTime(2018, 7, 1)),
                    id: id,
                    nodeId: "",
                    locked: false,
                    repository: null,
                    reactions: null
                ));

            mockGitBucketClient
                .Setup(g => g.Issue.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NewIssue>()))
                .ReturnsAsync((string owner, string repository, NewIssue newIssue) => new Octokit.Issue
                (
                    url: "",
                    htmlUrl: "",
                    commentsUrl: "",
                    eventsUrl: "",
                    number: 1,
                    state: ItemState.Open,
                    title: newIssue.Title,
                    body: newIssue.Body,
                    closedBy: null,
                    user: new User(),
                    labels: null,
                    assignee: new User(),
                    assignees: null,
                    milestone: null,
                    comments: 0,
                    pullRequest: null,
                    closedAt: null,
                    createdAt: new DateTimeOffset(new DateTime(2018, 7, 2)),
                    updatedAt: new DateTimeOffset(new DateTime(2018, 7, 2)),
                    id: 1,
                    nodeId: "",
                    locked: false,
                    repository: null,
                    reactions: null
                ));

            mockGitBucketClient
                .Setup(g => g.Issue.Comment.GetAllForIssue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.IssueComment>(new List<Octokit.IssueComment>{
                    new Octokit.IssueComment(
                        id:1,
                        nodeId:"",
                        url:"",
                        htmlUrl:"",
                        body:"This is a comment by root.",
                        createdAt:new DateTimeOffset(new DateTime(2018, 1, 1)),
                        updatedAt:new DateTimeOffset(new DateTime(2018, 1, 2)),
                        user:_user1,
                        reactions:new ReactionSummary()
                    )
                }));

            mockGitBucketClient
                .Setup(g => g.Issue.Comment.Create(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()
                ))
                .ReturnsAsync(new Octokit.IssueComment());

            var options = new IssueOptions
            {
                ExecutedDate = new DateTime(2018, 7, 1),
                Source = "root/test1",
                Destination = "root/test2",
                IssueNumber = 1,
                Type = "copy"
            };

            var console = new FakeConsole();
            var service = new IssueService(console);

            // When
            var result = await service.Execute(options, mockGitBucketClient.Object);

            // Then
            Assert.Equal(0, result);
            mockGitBucketClient.Verify(g => g.Issue.Get(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test1"),
                It.Is<int>(i => i == 1)));

            mockGitBucketClient.Verify(g => g.Issue.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<NewIssue>(i =>
                    i.Title == "Found a bug" &&
                    i.Body == "Original Issue Comment.\r\n\r\n*Copied from original issue: root/test1#1*")));

            mockGitBucketClient.Verify(g => g.Issue.Comment.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(n => n == 1),
                It.Is<string>(c => c == "This is a comment by root.")));
        }
    }
}