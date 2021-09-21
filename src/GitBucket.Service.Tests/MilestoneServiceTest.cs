using GitBucket.Core;
using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GitBucket.Service.Tests;

public class MilestoneServiceTest
{
    public FakeConsole FakeConsole { get; } = new FakeConsole("yes");

    [Fact]
    public async Task Should_Show_Open_Milestone()
    {
        // Given
        var dbContextOptions = new DbContextOptionsBuilder<GitBucketDbContext>()
            .UseInMemoryDatabase(databaseName: "Should_Show_Open_Milestone")
            .Options;

        var dbContext = new GitBucketDbContext(dbContextOptions);
        var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 1) };
        dbContext.Milestones.Add(new Milestone
        {
            MilestoneId = 1,
            Title = "v0.1.0",
            RepositoryName = "test1",
            DueDate = new DateTime(2018, 7, 8),
            ClosedDate = null,
            Description = "Implement xxx feature",
            UserName = "root",
            Issues = new[]
            {
                    new Issue
                    {
                        IssueId = 1,
                        AssignedUserName = "user1",
                        UserName = "root",
                        RepositoryName = "test1",
                        OpenedUserName = "root",
                        Title = "Implement xxx feature",
                    },
                    new Issue
                    {
                        IssueId = 2,
                        AssignedUserName = "user2",
                        UserName = "root",
                        RepositoryName = "test1",
                        OpenedUserName = "root",
                        Title = "Implement xxx feature",
                    },
                    new Issue
                    {
                        IssueId = 3,
                        AssignedUserName = "user1",
                        UserName = "root",
                        RepositoryName = "test1",
                        OpenedUserName = "root",
                        Title = "Implement xxx feature",
                    }
                }
        });

        dbContext.SaveChanges();

        var service = new MilestoneService(dbContext, FakeConsole);

        // When
        var result = await service.ShowMilestones(options);

        // Then
        Assert.Equal(0, result);
        Assert.Equal(3, FakeConsole.Messages.Count);
        Assert.Empty(FakeConsole.WarnMessages);
        Assert.Empty(FakeConsole.ErrorMessages);

        Assert.Equal("There are 1 open milestone.", FakeConsole.Messages[0]);
        Assert.Equal(string.Empty, FakeConsole.Messages[1]);

        Assert.Equal("* [root/test1], [v0.1.0], [2018/07/08], [Implement xxx feature], [user1, user2]", FakeConsole.Messages[2]);
    }

    [Fact]
    public async Task Should_Include_Closed_Milestone_If_Specified()
    {
        // Given
        var dbContextOptions = new DbContextOptionsBuilder<GitBucketDbContext>()
            .UseInMemoryDatabase(databaseName: "Should_Include_Closed_Milestone_If_Specified")
            .Options;

        var dbContext = new GitBucketDbContext(dbContextOptions);
        var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 1), IncludeClosed = true };
        dbContext.Milestones.Add(new Milestone
        {
            MilestoneId = 1,
            Title = "v0.1.0",
            RepositoryName = "test1",
            DueDate = new DateTime(2018, 7, 1),
            ClosedDate = new DateTime(2018, 7, 1),
            Description = "Implement xxx feature",
            UserName = "root",
            Issues = new[]
            {
                    new Issue
                    {
                        IssueId = 1,
                        AssignedUserName = "user1",
                        UserName = "root",
                        RepositoryName = "test1",
                        OpenedUserName = "root",
                        Title = "Implement xxx feature",
                    }
                }
        });

        dbContext.SaveChanges();

        var service = new MilestoneService(dbContext, FakeConsole);

        // When
        var result = await service.ShowMilestones(options);

        // Then
        Assert.Equal(0, result);
        Assert.Equal(3, FakeConsole.Messages.Count);
        Assert.Empty(FakeConsole.WarnMessages);
        Assert.Empty(FakeConsole.ErrorMessages);

        Assert.Equal("There are 1 milestone.", FakeConsole.Messages[0]);
        Assert.Equal(string.Empty, FakeConsole.Messages[1]);

        Assert.Equal("* [root/test1], [v0.1.0], [2018/07/01], [Implement xxx feature], [user1]", FakeConsole.Messages[2]);
    }

    [Fact]
    public async Task Should_Show_Expired_Milestones_With_Error()
    {
        // Given
        var dbContextOptions = new DbContextOptionsBuilder<GitBucketDbContext>()
            .UseInMemoryDatabase(databaseName: "Should_Show_Expired_Milestones_With_Error")
            .Options;

        var dbContext = new GitBucketDbContext(dbContextOptions);
        var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 1) };
        dbContext.Milestones.Add(new Milestone
        {
            MilestoneId = 1,
            Title = "v0.1.0",
            RepositoryName = "test1",
            DueDate = new DateTime(2018, 6, 30),
            ClosedDate = null,
            Description = "Implement xxx feature",
            UserName = "root",
            Issues = new[]
            {
                    new Issue
                    {
                        IssueId = 1,
                        AssignedUserName = "user1",
                        UserName = "root",
                        RepositoryName = "test1",
                        OpenedUserName = "root",
                        Title = "Implement xxx feature",
                    }
                }
        });

        dbContext.SaveChanges();

        var service = new MilestoneService(dbContext, FakeConsole);

        // When
        var result = await service.ShowMilestones(options);

        // Then
        Assert.Equal(0, result);
        Assert.Equal(2, FakeConsole.Messages.Count);
        Assert.Empty(FakeConsole.WarnMessages);
        Assert.Single(FakeConsole.ErrorMessages);

        Assert.Equal("There are 1 open milestone.", FakeConsole.Messages[0]);
        Assert.Equal(string.Empty, FakeConsole.Messages[1]);

        Assert.Equal("* [root/test1], [v0.1.0], [2018/06/30], [Implement xxx feature], [user1]", FakeConsole.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Show_Milestones_To_Be_Closed_In_A_Week_With_Warn()
    {
        // Given
        var dbContextOptions = new DbContextOptionsBuilder<GitBucketDbContext>()
            .UseInMemoryDatabase(databaseName: "Should_Show_Milestones_To_Be_Closed_In_A_Week_With_Warn")
            .Options;

        var dbContext = new GitBucketDbContext(dbContextOptions);
        var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 1) };
        dbContext.Milestones.Add(new Milestone
        {
            MilestoneId = 1,
            Title = "v0.1.0",
            RepositoryName = "test1",
            DueDate = new DateTime(2018, 7, 7),
            ClosedDate = null,
            Description = "Implement xxx feature",
            UserName = "root",
            Issues = new[]
            {
                    new Issue
                    {
                        IssueId = 1,
                        AssignedUserName = "user1",
                        UserName = "root",
                        RepositoryName = "test1",
                        OpenedUserName = "root",
                        Title = "Implement xxx feature",
                    }
                }
        });

        dbContext.SaveChanges();

        var service = new MilestoneService(dbContext, FakeConsole);

        // When
        var result = await service.ShowMilestones(options);

        // Then
        Assert.Equal(0, result);
        Assert.Equal(2, FakeConsole.Messages.Count);
        Assert.Single(FakeConsole.WarnMessages);
        Assert.Empty(FakeConsole.ErrorMessages);

        Assert.Equal("There are 1 open milestone.", FakeConsole.Messages[0]);
        Assert.Equal(string.Empty, FakeConsole.Messages[1]);

        Assert.Equal("* [root/test1], [v0.1.0], [2018/07/07], [Implement xxx feature], [user1]", FakeConsole.WarnMessages[0]);
    }

    [Fact]
    public async Task Should_Show_Milestones_With_Warn_If_DueDate_Equals_ExecutedDate()
    {
        // Given
        var dbContextOptions = new DbContextOptionsBuilder<GitBucketDbContext>()
            .UseInMemoryDatabase(databaseName: "Should_Show_Milestones_With_Warn_If_DueDate_Equals_ExecutedDate")
            .Options;

        var dbContext = new GitBucketDbContext(dbContextOptions);
        var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 1) };
        dbContext.Milestones.Add(new Milestone
        {
            MilestoneId = 1,
            Title = "v0.1.0",
            RepositoryName = "test1",
            DueDate = new DateTime(2018, 7, 1),
            ClosedDate = null,
            Description = "Implement xxx feature",
            UserName = "root",
            Issues = new[]
            {
                    new Issue
                    {
                        IssueId = 1,
                        AssignedUserName = "user1",
                        UserName = "root",
                        RepositoryName = "test1",
                        OpenedUserName = "root",
                        Title = "Implement xxx feature",
                    }
                }
        });

        dbContext.SaveChanges();

        var service = new MilestoneService(dbContext, FakeConsole);

        // When
        var result = await service.ShowMilestones(options);

        // Then
        Assert.Equal(0, result);
        Assert.Equal(2, FakeConsole.Messages.Count);
        Assert.Single(FakeConsole.WarnMessages);
        Assert.Empty(FakeConsole.ErrorMessages);

        Assert.Equal("There are 1 open milestone.", FakeConsole.Messages[0]);
        Assert.Equal(string.Empty, FakeConsole.Messages[1]);

        Assert.Equal("* [root/test1], [v0.1.0], [2018/07/01], [Implement xxx feature], [user1]", FakeConsole.WarnMessages[0]);
    }

    [Fact]
    public async Task Should_Show_Multiple_Milestones()
    {
        // Given
        var dbContextOptions = new DbContextOptionsBuilder<GitBucketDbContext>()
            .UseInMemoryDatabase(databaseName: "Should_Show_Multiple_Milestones")
            .Options;

        var dbContext = new GitBucketDbContext(dbContextOptions);
        var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 1), IncludeClosed = true };
        dbContext.Milestones.AddRange(new List<Core.Models.Milestone>
            {
                new Milestone
                {
                    MilestoneId = 1,
                    Title = "v0.1.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 6, 30),
                    ClosedDate = null,
                    Description = "Error",
                    UserName = "root",
                    Issues = new[]
                    {
                        new Issue
                        {
                            IssueId = 1,
                            AssignedUserName = "user1",
                            UserName = "root",
                            RepositoryName = "test1",
                            OpenedUserName = "root",
                            Title = "Implement xxx feature",
                        }
                    }
                },
                new Milestone
                {
                    MilestoneId = 2,
                    Title = "v0.2.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 7, 1),
                    ClosedDate = new DateTime(2018, 7, 1),
                    Description = "Closed",
                    UserName = "root",
                    Issues = new[]
                    {
                        new Issue
                        {
                            IssueId = 2,
                            AssignedUserName = "user1",
                            UserName = "root",
                            RepositoryName = "test1",
                            OpenedUserName = "root",
                            Title = "Implement xxx feature",
                        }
                    }
                },
                new Milestone
                {
                    MilestoneId = 3,
                    Title = "v0.3.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 7, 7),
                    ClosedDate = null,
                    Description = "Warn",
                    UserName = "root",
                    Issues = new[]
                    {
                        new Issue
                        {
                            IssueId = 3,
                            AssignedUserName = "user1",
                            UserName = "root",
                            RepositoryName = "test1",
                            OpenedUserName = "root",
                            Title = "Implement xxx feature",
                        }
                    }
                },
                new Milestone
                {
                    MilestoneId = 4,
                    Title = "v0.4.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 7, 8),
                    ClosedDate = null,
                    Description = "Info",
                    UserName = "root",
                    Issues = new[]
                    {
                        new Issue
                        {
                            IssueId = 4,
                            AssignedUserName = "user1",
                            UserName = "root",
                            RepositoryName = "test1",
                            OpenedUserName = "root",
                            Title = "Implement xxx feature",
                        }
                    }
                }
            });

        dbContext.SaveChanges();

        var service = new MilestoneService(dbContext, FakeConsole);

        // When
        var result = await service.ShowMilestones(options);

        // Then
        Assert.Equal(0, result);
        Assert.Equal(4, FakeConsole.Messages.Count);
        Assert.Single(FakeConsole.WarnMessages);
        Assert.Single(FakeConsole.ErrorMessages);

        Assert.Equal("There are 4 milestones.", FakeConsole.Messages[0]);
        Assert.Equal(string.Empty, FakeConsole.Messages[1]);

        Assert.Equal("* [root/test1], [v0.1.0], [2018/06/30], [Error], [user1]", FakeConsole.ErrorMessages[0]);
        Assert.Equal("* [root/test1], [v0.2.0], [2018/07/01], [Closed], [user1]", FakeConsole.Messages[2]);
        Assert.Equal("* [root/test1], [v0.3.0], [2018/07/07], [Warn], [user1]", FakeConsole.WarnMessages[0]);
        Assert.Equal("* [root/test1], [v0.4.0], [2018/07/08], [Info], [user1]", FakeConsole.Messages[3]);
    }

    [Fact]
    public async Task Should_Return_If_No_Milestone()
    {
        // Given
        var dbContextOptions = new DbContextOptionsBuilder<GitBucketDbContext>()
            .UseInMemoryDatabase(databaseName: "Should_Return_If_No_Milestone")
            .Options;

        var dbContext = new GitBucketDbContext(dbContextOptions);
        var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 29) };
        var service = new MilestoneService(dbContext, FakeConsole);

        // When
        var result = await service.ShowMilestones(options);

        // Then
        Assert.Equal(0, result);
        Assert.Single(FakeConsole.Messages);
        Assert.Empty(FakeConsole.WarnMessages);
        Assert.Empty(FakeConsole.ErrorMessages);

        Assert.Equal("There are no milestone.", FakeConsole.Messages[0]);
    }
}
