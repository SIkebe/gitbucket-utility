using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GitBucket.Core;
using Moq;
using Octokit;
using Xunit;

namespace GitBucket.Service.Tests
{
    public class IssueServiceTest
    {
        private readonly User _rootUser = new
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

        private readonly User _user1 = new
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
            var labels = new List<Label>
            {
                new Label(id:1, url:"", name:"bug", nodeId:"1", color:"#fc2929", description:"bug", @default:true)
            };

            var mockGitBucketClient = new Mock<IGitHubClient>(MockBehavior.Strict);

            mockGitBucketClient
                .Setup(m => m.Repository.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string owner, string repository) => new FakeRepository
                (
                    owner: _rootUser,
                    repository: repository,
                    createdAt: new DateTimeOffset(new DateTime(2018, 7, 1))
                ));

            mockGitBucketClient
                .Setup(g => g.Issue.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((string owner, string repository, int id) => new Octokit.Issue
                (
                    url: "",
                    htmlUrl: "",
                    commentsUrl: "",
                    eventsUrl: "",
                    number: id,
                    state: ItemState.Open,
                    title: "Found a bug",
                    body: "Original Issue Comment.",
                    closedBy: null,
                    user: _rootUser,
                    labels: labels,
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
                .Setup(g => g.Issue.Labels.AddToIssue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string[]>()))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.Label>(labels));

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
                        reactions:new ReactionSummary(),
                        authorAssociation:AuthorAssociation.Collaborator
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
                Source = new[] { "root", "test1" },
                Destination = new[] { "root", "test2" },
                IssueNumbers = new[] { 1 }
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
                    i.Body == $"*From @root on 2018-07-01 00:00:00*{Environment.NewLine}{Environment.NewLine}Original Issue Comment.{Environment.NewLine}{Environment.NewLine}*Copied from original issue: root/test1#1*")));

            mockGitBucketClient.Verify(g => g.Issue.Labels.AddToIssue(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(i => i == 1),
                It.Is<string[]>(i => i.Single() == "bug")));

            mockGitBucketClient.Verify(g => g.Issue.Comment.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(n => n == 1),
                It.Is<string>(c => c == $"*From @user1 on 2018-01-01 00:00:00*{Environment.NewLine}{Environment.NewLine}This is a comment by root.")));

            mockGitBucketClient.Verify(g => g.Issue.Comment.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test1"),
                It.Is<int>(n => n == 1),
                It.Is<string>(c => c == "*This issue was moved to root/test2#1*")));
        }

        [Fact]
        public async Task Should_Move_Multiple_Issues_To_Same_Owner_Repository()
        {
            // Given
            var labels = new List<Label>
            {
                new Label(id:1, url:"", name:"bug", nodeId:"1", color:"#fc2929", description:"bug", @default:true)
            };

            var mockGitBucketClient = new Mock<IGitHubClient>(MockBehavior.Strict);

            mockGitBucketClient
                .Setup(m => m.Repository.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string owner, string repository) => new FakeRepository
                (
                    owner: _rootUser,
                    repository: repository,
                    createdAt: new DateTimeOffset(new DateTime(2018, 7, 1))
                ));

            mockGitBucketClient
                .Setup(g => g.Issue.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((string owner, string repository, int id) => new Octokit.Issue
                (
                    url: "",
                    htmlUrl: "",
                    commentsUrl: "",
                    eventsUrl: "",
                    number: id,
                    state: ItemState.Open,
                    title: "Found a bug",
                    body: "Original Issue Comment.",
                    closedBy: null,
                    user: _rootUser,
                    labels: labels,
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

            var invocationCount = 0;
            mockGitBucketClient
                .Setup(g => g.Issue.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NewIssue>()))
                .Callback(() => invocationCount++)
                .ReturnsAsync((string owner, string repository, NewIssue newIssue) => new Octokit.Issue
                (
                    url: "",
                    htmlUrl: "",
                    commentsUrl: "",
                    eventsUrl: "",
                    number: invocationCount,
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
                    id: invocationCount,
                    nodeId: "",
                    locked: false,
                    repository: null,
                    reactions: null
                ));

            mockGitBucketClient
                .Setup(g => g.Issue.Labels.AddToIssue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string[]>()))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.Label>(labels));

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
                        reactions:new ReactionSummary(),
                        authorAssociation:AuthorAssociation.Collaborator
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
                Source = new[] { "root", "test1" },
                Destination = new[] { "root", "test2" },
                IssueNumbers = new[] { 1, 2 }
            };

            var console = new FakeConsole();
            var service = new IssueService(console);

            // When
            var result = await service.Execute(options, mockGitBucketClient.Object);

            // Then
            Assert.Equal(0, result);

            // First issue
            mockGitBucketClient.Verify(g => g.Issue.Get(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test1"),
                It.Is<int>(i => i == 1)));

            mockGitBucketClient.Verify(g => g.Issue.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<NewIssue>(i =>
                    i.Title == "Found a bug" &&
                    i.Body == $"*From @root on 2018-07-01 00:00:00*{Environment.NewLine}{Environment.NewLine}Original Issue Comment.{Environment.NewLine}{Environment.NewLine}*Copied from original issue: root/test1#1*")));

            mockGitBucketClient.Verify(g => g.Issue.Labels.AddToIssue(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(i => i == 1),
                It.Is<string[]>(i => i.Single() == "bug")));

            mockGitBucketClient.Verify(g => g.Issue.Comment.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(n => n == 1),
                It.Is<string>(c => c == $"*From @user1 on 2018-01-01 00:00:00*{Environment.NewLine}{Environment.NewLine}This is a comment by root.")));

            mockGitBucketClient.Verify(g => g.Issue.Comment.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test1"),
                It.Is<int>(n => n == 1),
                It.Is<string>(c => c == "*This issue was moved to root/test2#1*")));

            // Second issue
            mockGitBucketClient.Verify(g => g.Issue.Get(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test1"),
                It.Is<int>(i => i == 2)));

            mockGitBucketClient.Verify(g => g.Issue.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<NewIssue>(i =>
                    i.Title == "Found a bug" &&
                    i.Body == $"*From @root on 2018-07-01 00:00:00*{Environment.NewLine}{Environment.NewLine}Original Issue Comment.{Environment.NewLine}{Environment.NewLine}*Copied from original issue: root/test1#2*")));

            mockGitBucketClient.Verify(g => g.Issue.Labels.AddToIssue(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(i => i == 2),
                It.Is<string[]>(i => i.Single() == "bug")));

            mockGitBucketClient.Verify(g => g.Issue.Comment.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(n => n == 2),
                It.Is<string>(c => c == $"*From @user1 on 2018-01-01 00:00:00*{Environment.NewLine}{Environment.NewLine}This is a comment by root.")));

            mockGitBucketClient.Verify(g => g.Issue.Comment.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test1"),
                It.Is<int>(n => n == 2),
                It.Is<string>(c => c == "*This issue was moved to root/test2#2*")));
        }

        [Fact]
        public async Task Throw_If_Repository_Not_Exists()
        {
            // Given
            var mockGitBucketClient = new Mock<IGitHubClient>(MockBehavior.Strict);

            mockGitBucketClient
                .Setup(m => m.Repository.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new NotFoundException("Not Found", HttpStatusCode.NotFound));

            var options = new IssueOptions
            {
                ExecutedDate = new DateTime(2018, 7, 1),
                Source = new[] { "root", "test1" },
                Destination = new[] { "root", "test2" },
                IssueNumbers = new[] { 1 },
                Type = "copy"
            };

            var console = new FakeConsole();
            var service = new IssueService(console);

            // When
            var result = await Assert.ThrowsAsync<NotFoundException>(() => service.Execute(options, mockGitBucketClient.Object));

            // Then
            Assert.Equal("Not Found", result.Message);
            Assert.Single(console.ErrorMessages);
            Assert.Equal(@"Repository root/test1 does not exist.", console.ErrorMessages.First());
        }

        [Fact]
        public async Task Unsupported_Issue_Type()
        {
            // Given
            var mockGitBucketClient = new Mock<IGitHubClient>(MockBehavior.Strict);

            mockGitBucketClient
                .Setup(m => m.Repository.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string owner, string repository) => new FakeRepository
                (
                    owner: _rootUser,
                    repository: repository,
                    createdAt: new DateTimeOffset(new DateTime(2018, 7, 1))
                ));

            var options = new IssueOptions
            {
                ExecutedDate = new DateTime(2018, 7, 1),
                Source = new[] { "root", "test1" },
                Destination = new[] { "root", "test2" },
                IssueNumbers = new[] { 1 },
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

        [Fact]
        public async Task Should_Copy_Issue_To_Same_Owner_Repository()
        {
            // Given
            var labels = new List<Label>
            {
                new Label(id:1, url:"", name:"bug", nodeId:"1", color:"#fc2929", description:"bug", @default:true)
            };

            var mockGitBucketClient = new Mock<IGitHubClient>(MockBehavior.Strict);

            mockGitBucketClient
                .Setup(m => m.Repository.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string owner, string repository) => new FakeRepository
                (
                    owner: _rootUser,
                    repository: repository,
                    createdAt: new DateTimeOffset(new DateTime(2018, 7, 1))
                ));

            mockGitBucketClient
                .Setup(g => g.Issue.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((string owner, string repository, int id) => new Octokit.Issue
                (
                    url: "",
                    htmlUrl: "",
                    commentsUrl: "",
                    eventsUrl: "",
                    number: id,
                    state: ItemState.Open,
                    title: "Found a bug",
                    body: "Original Issue Comment.",
                    closedBy: null,
                    user: _rootUser,
                    labels: labels,
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
                .Setup(g => g.Issue.Labels.AddToIssue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string[]>()))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.Label>(labels));

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
                        reactions:new ReactionSummary(),
                        authorAssociation:AuthorAssociation.Collaborator
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
                Source = new[] { "root", "test1" },
                Destination = new[] { "root", "test2" },
                IssueNumbers = new[] { 1 },
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
                    i.Body == $"Original Issue Comment.{Environment.NewLine}{Environment.NewLine}*Copied from original issue: root/test1#1*")));

            mockGitBucketClient.Verify(g => g.Issue.Labels.AddToIssue(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(i => i == 1),
                It.Is<string[]>(i => i.Single() == "bug")));

            mockGitBucketClient.Verify(g => g.Issue.Comment.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(n => n == 1),
                It.Is<string>(c => c == "This is a comment by root.")));
        }

        [Fact]
        public async Task Should_Copy_Multiple_Issues_To_Same_Owner_Repository()
        {
            // Given
            var labels = new List<Label>
            {
                new Label(id:1, url:"", name:"bug", nodeId:"1", color:"#fc2929", description:"bug", @default:true)
            };

            var mockGitBucketClient = new Mock<IGitHubClient>(MockBehavior.Strict);

            mockGitBucketClient
                .Setup(m => m.Repository.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string owner, string repository) => new FakeRepository
                (
                    owner: _rootUser,
                    repository: repository,
                    createdAt: new DateTimeOffset(new DateTime(2018, 7, 1))
                ));

            mockGitBucketClient
                .Setup(g => g.Issue.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((string owner, string repository, int id) => new Octokit.Issue
                (
                    url: "",
                    htmlUrl: "",
                    commentsUrl: "",
                    eventsUrl: "",
                    number: id,
                    state: ItemState.Open,
                    title: "Found a bug",
                    body: "Original Issue Comment.",
                    closedBy: null,
                    user: _rootUser,
                    labels: labels,
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

            var invocationCount = 0;
            mockGitBucketClient
                .Setup(g => g.Issue.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NewIssue>()))
                .Callback(() => invocationCount++)
                .ReturnsAsync((string owner, string repository, NewIssue newIssue) => new Octokit.Issue
                (
                    url: "",
                    htmlUrl: "",
                    commentsUrl: "",
                    eventsUrl: "",
                    number: invocationCount,
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
                    id: invocationCount,
                    nodeId: "",
                    locked: false,
                    repository: null,
                    reactions: null
                ));

            mockGitBucketClient
                .Setup(g => g.Issue.Labels.AddToIssue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string[]>()))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.Label>(labels));

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
                        reactions:new ReactionSummary(),
                        authorAssociation:AuthorAssociation.Collaborator
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
                Source = new[] { "root", "test1" },
                Destination = new[] { "root", "test2" },
                IssueNumbers = new[] { 1, 2 },
                Type = "copy"
            };

            var console = new FakeConsole();
            var service = new IssueService(console);

            // When
            var result = await service.Execute(options, mockGitBucketClient.Object);

            // Then
            Assert.Equal(0, result);

            // First issue
            mockGitBucketClient.Verify(g => g.Issue.Get(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test1"),
                It.Is<int>(i => i == 1)));

            mockGitBucketClient.Verify(g => g.Issue.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<NewIssue>(i =>
                    i.Title == "Found a bug" &&
                    i.Body == $"Original Issue Comment.{Environment.NewLine}{Environment.NewLine}*Copied from original issue: root/test1#1*")));

            mockGitBucketClient.Verify(g => g.Issue.Labels.AddToIssue(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(i => i == 1),
                It.Is<string[]>(i => i.Single() == "bug")));

            mockGitBucketClient.Verify(g => g.Issue.Comment.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(n => n == 1),
                It.Is<string>(c => c == "This is a comment by root.")));

            // Second issue
            mockGitBucketClient.Verify(g => g.Issue.Get(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test1"),
                It.Is<int>(i => i == 2)));

            mockGitBucketClient.Verify(g => g.Issue.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<NewIssue>(i =>
                    i.Title == "Found a bug" &&
                    i.Body == $"Original Issue Comment.{Environment.NewLine}{Environment.NewLine}*Copied from original issue: root/test1#2*")));

            mockGitBucketClient.Verify(g => g.Issue.Comment.Create(
                It.Is<string>(o => o == "root"),
                It.Is<string>(r => r == "test2"),
                It.Is<int>(n => n == 2),
                It.Is<string>(c => c == "This is a comment by root.")));
        }
    }
}
