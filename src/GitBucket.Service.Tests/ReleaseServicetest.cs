using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GitBucket.Core;
using Microsoft.EntityFrameworkCore;
using Moq;
using Octokit;
using Xunit;

namespace GitBucket.Service.Tests
{
    public class ReleaseServiceTest
    {
        public FakeConsole FakeConsole { get; } = new FakeConsole("yes");

        [Fact]
        public async void Milestone_Has_No_Issue()
        {
            // Given
            var dbContextOptions = new DbContextOptionsBuilder<GitBucketDbContext>()
                .UseInMemoryDatabase(databaseName: "Milestone_Has_No_Issue")
                .Options;

            var dbContext = new GitBucketDbContext(dbContextOptions);
            var options = new ReleaseOptions { MileStone = "v1.0.0", Owner = "root", Repository = "test" };
            var service = new ReleaseService(dbContext, FakeConsole);

            // When
            var result = await service.Execute(options, new Mock<IGitHubClient>().Object);

            // Then
            Assert.Equal(1, result);
            Assert.Empty(FakeConsole.Messages);
            Assert.Single(FakeConsole.WarnMessages);
            Assert.Empty(FakeConsole.ErrorMessages);
            Assert.Equal("There are no issues related to \"v1.0.0\".", FakeConsole.WarnMessages[0]);
        }

        [Fact]
        public async void Milestone_Has_No_PullRequest()
        {
            // Given
            var dbContextOptions = new DbContextOptionsBuilder<GitBucketDbContext>()
                .UseInMemoryDatabase(databaseName: "Milestone_Has_No_PullRequest")
                .Options;

            var dbContext = new GitBucketDbContext(dbContextOptions);
            var options = new ReleaseOptions { FromPullRequest = true, MileStone = "v1.0.0", Owner = "root", Repository = "test" };
            var service = new ReleaseService(dbContext, FakeConsole);

            // When
            var result = await service.Execute(options, new Mock<IGitHubClient>().Object);

            // Then
            Assert.Equal(1, result);
            Assert.Empty(FakeConsole.Messages);
            Assert.Single(FakeConsole.WarnMessages);
            Assert.Empty(FakeConsole.ErrorMessages);
            Assert.Equal("There are no pull requests related to \"v1.0.0\".", FakeConsole.WarnMessages[0]);
        }

        [Fact]
        public async void Milestone_Has_Unclosed_Issue()
        {
            // Given
            var options = new ReleaseOptions { MileStone = "v1.0.0", Owner = "root", Repository = "test" };
            var dbContextOptions = new DbContextOptionsBuilder<GitBucketDbContext>()
                .UseInMemoryDatabase(databaseName: "Milestone_Has_Unclosed_Issue")
                .Options;

            var dbContext = new GitBucketDbContext(dbContextOptions);
            dbContext.Issue.Add(new Core.Models.Issue
            {
                Closed = false,
                Milestone = new Core.Models.Milestone
                {
                    RepositoryName = options.Repository,
                    Title = options.MileStone,
                    UserName = options.Owner,
                },
                RepositoryName = options.Repository,
                UserName = options.Owner,
            });

            dbContext.SaveChanges();

            var console = new FakeConsole("no");
            var service = new ReleaseService(dbContext, console);

            // When
            var result = await service.Execute(options, new Mock<IGitHubClient>().Object);

            // Then
            Assert.Equal(1, result);
            Assert.Empty(console.Messages);
            Assert.Equal(2, console.WarnMessages.Count);
            Assert.Empty(console.ErrorMessages);
            Assert.Equal("There are unclosed issues in \"v1.0.0\".", console.WarnMessages[0]);
            Assert.Equal("Do you want to continue?([Y]es/[N]o): no", console.WarnMessages[1]);
        }

        [Fact]
        public async void Milestone_Has_Issue_Without_Labels()
        {
            // Given
            var options = new ReleaseOptions { MileStone = "v1.0.0", Owner = "root", Repository = "test" };
            var dbContextOptions = new DbContextOptionsBuilder<GitBucketDbContext>()
                .UseInMemoryDatabase(databaseName: "Milestone_Has_Issue_Without_Labels")
                .Options;

            var dbContext = new GitBucketDbContext(dbContextOptions);
            dbContext.Issue.Add(new Core.Models.Issue
            {
                Milestone = new Core.Models.Milestone
                {
                    RepositoryName = options.Repository,
                    Title = options.MileStone,
                    UserName = options.Owner,
                },
                RepositoryName = options.Repository,
                UserName = options.Owner,
            });

            dbContext.SaveChanges();

            var service = new ReleaseService(dbContext, FakeConsole);

            // When
            var result = await service.Execute(options, new Mock<IGitHubClient>().Object);

            // Then
            Assert.Equal(1, result);
            Assert.Single(FakeConsole.Messages);
            Assert.Equal("", FakeConsole.Messages[0]);

            Assert.Equal(3, FakeConsole.WarnMessages.Count);
            Assert.Equal("There are unclosed issues in \"v1.0.0\".", FakeConsole.WarnMessages[0]);
            Assert.Equal("Do you want to continue?([Y]es/[N]o): yes", FakeConsole.WarnMessages[1]);
            Assert.Equal("There are issues which have no labels in \"v1.0.0\".", FakeConsole.WarnMessages[2]);

            Assert.Empty(FakeConsole.ErrorMessages);
        }

        [Fact]
        public async void PullRequest_Already_Exists()
        {
            // Given
            var options = new ReleaseOptions { CreatePullRequest = true, MileStone = "v1.0.0", Owner = "root", Repository = "test" };
            var dbContext = EnsureDbCreated(options);
            var service = new ReleaseService(dbContext, FakeConsole);
            var gitbucketClient = new Mock<IGitHubClient>();
            gitbucketClient
                .Setup(g => g.PullRequest.GetAllForRepository(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.PullRequest>(new List<Octokit.PullRequest>
                {
                    new FakePullRequest(new FakeGitReference("develop"), new FakeGitReference("master"))
                }));

            // When
            var result = await service.Execute(options, gitbucketClient.Object);

            // Then
            Assert.Equal(1, result);
            Assert.Empty(FakeConsole.Messages);
            Assert.Single(FakeConsole.WarnMessages);
            Assert.Equal("A pull request already exists for root:develop.", FakeConsole.WarnMessages[0]);
            Assert.Empty(FakeConsole.ErrorMessages);
        }

        [Fact]
        public async void Should_Create_PullRequest()
        {
            // Given
            var options = new ReleaseOptions { CreatePullRequest = true, MileStone = "v1.0.0", Owner = "root", Repository = "test" };
            var dbContext = EnsureDbCreated(options);
            var service = new ReleaseService(dbContext, FakeConsole);
            var gitbucketClient = new Mock<IGitHubClient>();
            gitbucketClient
                .SetupSequence(g => g.PullRequest.GetAllForRepository(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.PullRequest>(new List<Octokit.PullRequest>()))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.PullRequest>(new List<Octokit.PullRequest>
                {
                    new FakePullRequest(new FakeGitReference("develop"), new FakeGitReference("master"))
                }));

            gitbucketClient
                .Setup(g => g.PullRequest.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NewPullRequest>()))
                .ThrowsAsync(new InvalidCastException("Ignore InvalidCastException because of escaped response."));

            // When
            var result = await service.Execute(options, gitbucketClient.Object);

            // Then
            Assert.Equal(0, result);

            gitbucketClient
                .Verify(g => g.PullRequest.Create(
                    It.Is<string>(o => o == "root"),
                    It.Is<string>(r => r == "test"),
                    It.Is<NewPullRequest>(p =>
                        p.Title == "v1.0.0" &&
                        p.Head == "develop" &&
                        p.Base == "master" &&
                        p.Body == @"As part of this release we had 3 issues closed.
The highest priority among them is ""high"".

### Bug
* Found a bug! #1
* Another bug #2

### Enhancement
* Some improvement on build #3

")));

            Assert.Single(FakeConsole.Messages);
            Assert.Equal("A new pull request has been successfully created!", FakeConsole.Messages[0]);
            Assert.Empty(FakeConsole.WarnMessages);
            Assert.Empty(FakeConsole.ErrorMessages);
        }

        [Fact]
        public async void Should_Create_Draft_PullRequest()
        {
            // Given
            var options = new ReleaseOptions { Draft = true, CreatePullRequest = true, MileStone = "v1.0.0", Owner = "root", Repository = "test" };
            var dbContext = EnsureDbCreated(options);
            dbContext.PullRequest.AddRange(new List<Core.Models.PullRequest>
            {
                new Core.Models.PullRequest { UserName = "root", RepositoryName = "test", RequestBranch = "develop", Branch = "master", IssueId = 1 },
                new Core.Models.PullRequest { UserName = "root", RepositoryName = "test", RequestBranch = "develop", Branch = "master", IssueId = 2 }
            });
            dbContext.SaveChanges();

            var service = new ReleaseService(dbContext, FakeConsole);
            var gitbucketClient = new Mock<IGitHubClient>();
            gitbucketClient
                .SetupSequence(g => g.PullRequest.GetAllForRepository(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.PullRequest>(new List<Octokit.PullRequest>()))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.PullRequest>(new List<Octokit.PullRequest>
                {
                    new FakePullRequest(new FakeGitReference("develop"), new FakeGitReference("master"), 2)
                }));

            gitbucketClient
                .Setup(g => g.PullRequest.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NewPullRequest>()))
                .ThrowsAsync(new InvalidCastException("Ignore InvalidCastException because of escaped response."));

            // When
            var result = await service.Execute(options, gitbucketClient.Object);

            // Then
            Assert.Equal(0, result);

            gitbucketClient
                .Verify(g => g.PullRequest.Create(
                    It.Is<string>(o => o == "root"),
                    It.Is<string>(r => r == "test"),
                    It.Is<NewPullRequest>(p =>
                        p.Title == "v1.0.0" &&
                        p.Head == "develop" &&
                        p.Base == "master" &&
                        p.Body == @"As part of this release we had 3 issues closed.
The highest priority among them is ""high"".

### Bug
* Found a bug! #1
* Another bug #2

### Enhancement
* Some improvement on build #3

")));

            Assert.Single(FakeConsole.Messages);
            Assert.Equal("A new pull request has been successfully created!", FakeConsole.Messages[0]);
            Assert.Empty(FakeConsole.WarnMessages);
            Assert.Empty(FakeConsole.ErrorMessages);

            var isDraft = dbContext.PullRequest.Where(p => p.IssueId == 2).Select(p => p.IsDraft).Single();
            Assert.True(isDraft);
        }

        [Fact]
        public async void Should_Create_PullRequest_With_Different_Options()
        {
            // Given
            var options = new ReleaseOptions
            {
                Base = "release/v1.0.0",
                CreatePullRequest = true,
                FromPullRequest = true,
                Head = "master2",
                MileStone = "v1.0.0",
                Owner = "root",
                Repository = "test",
                Title = "Amazing PR",
            };

            var dbContext = EnsureDbCreated(options);
            var service = new ReleaseService(dbContext, FakeConsole);
            var gitbucketClient = new Mock<IGitHubClient>();
            gitbucketClient
                .SetupSequence(g => g.PullRequest.GetAllForRepository(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.PullRequest>(new List<Octokit.PullRequest>
                {
                    new FakePullRequest(new FakeGitReference("improve-performance"), new FakeGitReference("master"))
                }))
                .ReturnsAsync(new ReadOnlyCollection<Octokit.PullRequest>(new List<Octokit.PullRequest>
                {
                    new FakePullRequest(new FakeGitReference("improve-performance"), new FakeGitReference("master")),
                    new FakePullRequest(new FakeGitReference("release/v1.0.0"), new FakeGitReference("master"), 2)
                }));

            gitbucketClient
                .Setup(g => g.PullRequest.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NewPullRequest>()))
                .ThrowsAsync(new InvalidCastException("Ignore InvalidCastException because of escaped response."));

            // When
            var result = await service.Execute(options, gitbucketClient.Object);

            // Then
            Assert.Equal(0, result);

            gitbucketClient
                .Verify(g => g.PullRequest.Create(
                    It.Is<string>(o => o == "root"),
                    It.Is<string>(r => r == "test"),
                    It.Is<NewPullRequest>(p =>
                        p.Title == "Amazing PR" &&
                        p.Head == "master2" &&
                        p.Base == "release/v1.0.0" &&
                        p.Body == @"As part of this release we had 1 pull requests closed.
The highest priority among them is ""default"".

### Bug
* Fix a bug #4

")));

            Assert.Single(FakeConsole.Messages);
            Assert.Equal("A new pull request has been successfully created!", FakeConsole.Messages[0]);
            Assert.Empty(FakeConsole.WarnMessages);
            Assert.Empty(FakeConsole.ErrorMessages);
        }

        [Fact]
        public async void Should_Output_ReleaseNote()
        {
            // Given
            var options = new ReleaseOptions { MileStone = "v1.0.0", Owner = "root", Repository = "test" };
            var dbContext = EnsureDbCreated(options);
            var service = new ReleaseService(dbContext, FakeConsole);

            // When
            var result = await service.Execute(options, new Mock<IGitHubClient>().Object);

            // Then
            Assert.Equal(0, result);
            Assert.Single(FakeConsole.Messages);
            Assert.Equal(
                @"As part of this release we had 3 issues closed.
The highest priority among them is ""high"".

### Bug
* Found a bug! #1
* Another bug #2

### Enhancement
* Some improvement on build #3

", FakeConsole.Messages[0]);

            Assert.Empty(FakeConsole.WarnMessages);
            Assert.Empty(FakeConsole.ErrorMessages);
        }

        private static GitBucketDbContext EnsureDbCreated(ReleaseOptions options)
        {
            var dbContextOptions = new DbContextOptionsBuilder<GitBucketDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new GitBucketDbContext(dbContextOptions);

            var milestone = new Core.Models.Milestone
            {
                RepositoryName = options.Repository,
                Title = options.MileStone,
                UserName = options.Owner,
            };

            var highPriority = new Core.Models.Priority
            {
                Ordering = 0,
                PriorityName = "high",
                RepositoryName = options.Repository,
                UserName = options.Owner,
            };

            var defaultPriority = new Core.Models.Priority
            {
                Ordering = 1,
                PriorityName = "default",
                RepositoryName = options.Repository,
                UserName = options.Owner,
            };

            dbContext.Issue.AddRange(new List<Core.Models.Issue>
            {
                new Core.Models.Issue
                {
                    Closed = true,
                    IssueId = 1,
                    Milestone = milestone,
                    Priority = highPriority,
                    RepositoryName = options.Repository,
                    Title = "Found a bug!",
                    UserName = options.Owner,
                },
                new Core.Models.Issue
                {
                    Closed = true,
                    IssueId = 2,
                    Milestone = milestone,
                    Priority = defaultPriority,
                    RepositoryName = options.Repository,
                    Title = "Another bug",
                    UserName = options.Owner,
                },
                new Core.Models.Issue
                {
                    Closed = true,
                    IssueId = 3,
                    Milestone = milestone,
                    Priority = defaultPriority,
                    RepositoryName = options.Repository,
                    Title = "Some improvement on build",
                    UserName = options.Owner,
                },
                new Core.Models.Issue
                {
                    Closed = true,
                    IssueId = 4,
                    Milestone = milestone,
                    Priority = defaultPriority,
                    PullRequest = true,
                    RepositoryName = options.Repository,
                    Title = "Fix a bug",
                    UserName = options.Owner,
                }
            });

            dbContext.IssueLabel.AddRange(new List<Core.Models.IssueLabel>
            {
                new Core.Models.IssueLabel
                {
                    IssueId = 1,
                    LabelId = 10,
                    RepositoryName = options.Repository,
                    UserName = options.Owner,
                },
                new Core.Models.IssueLabel
                {
                    IssueId = 2,
                    LabelId = 10,
                    RepositoryName = options.Repository,
                    UserName = options.Owner,
                },
                new Core.Models.IssueLabel
                {
                    IssueId = 3,
                    LabelId = 30,
                    RepositoryName = options.Repository,
                    UserName = options.Owner,
                },
                new Core.Models.IssueLabel
                {
                    IssueId = 4,
                    LabelId = 10,
                    RepositoryName = options.Repository,
                    UserName = options.Owner,
                }
            });

            dbContext.Label.AddRange(new List<Core.Models.Label>
            {
                new Core.Models.Label
                {
                    LabelId = 10,
                    LabelName = "Bug",
                    RepositoryName = options.Repository,
                    UserName = options.Owner,
                },
                new Core.Models.Label
                {
                    LabelId = 30,
                    LabelName = "Enhancement",
                    RepositoryName = options.Repository,
                    UserName = options.Owner,
                }
            });

            dbContext.SaveChanges();
            return dbContext;
        }
    }

    public sealed class FakePullRequest : Octokit.PullRequest
    {
        public FakePullRequest(GitReference head, GitReference @base, int number = 1)
        {
            Head = head;
            Base = @base;
            Number = number;
        }
    }

    public sealed class FakeGitReference : Octokit.GitReference
    {
        public FakeGitReference(string @ref) => Ref = @ref;
    }
}
