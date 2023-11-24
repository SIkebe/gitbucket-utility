using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GitBucket.Service.Tests;

public class MilestoneExtensionsTest
{
    [Fact]
    public void Should_Return_Formatted_Milestone()
    {
        // Given
        var milestone = new Milestone
        {
            Description = "Imprement xxx feature.",
            DueDate = new DateTime(2019, 1, 15),
            RepositoryName = "test",
            UserName = "root",
            Title = "v1.0.0",
        };

        var issue = new Issue();
        issue.IssueAssignees.Add(new IssueAssignee { AssigneeUserName = "user1" });
        milestone.Issues.Add(issue);

        // When
        var formatted = milestone.Format();

        // Then
        Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [Imprement xxx feature.], [user1]", formatted);
    }

    [Fact]
    public void Should_Return_Formatted_Milestone_With_Multiple_Issues()
    {
        // Given
        var milestone = new Milestone
        {
            Description = "Imprement xxx feature.",
            DueDate = new DateTime(2019, 1, 15),
            RepositoryName = "test",
            UserName = "root",
            Title = "v1.0.0",
        };

        var issue1 = new Issue();
        var issue2 = new Issue();
        var issue3 = new Issue();
        var issue4 = new Issue();
        issue1.IssueAssignees.Add(new IssueAssignee { AssigneeUserName = "user1" });
        issue2.IssueAssignees.Add(new IssueAssignee { AssigneeUserName = "user3" });
        issue3.IssueAssignees.Add(new IssueAssignee { AssigneeUserName = "user2" });
        issue4.IssueAssignees.Add(new IssueAssignee { AssigneeUserName = "user2" });
        milestone.Issues.Add(issue1);
        milestone.Issues.Add(issue2);
        milestone.Issues.Add(issue3);
        milestone.Issues.Add(issue4);

        // When
        var formatted = milestone.Format();

        // Then
        Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [Imprement xxx feature.], [user1, user2, user3]", formatted);
    }

    [Fact]
    public void Should_Return_Formatted_Milestone_With_Long_Description()
    {
        // Given
        var milestone = new Milestone
        {
            Description = "Long-Long-Long-Long-Long-Long-Long-Long-Long-Long-Long-TItle",
            DueDate = new DateTime(2019, 1, 15),
            RepositoryName = "test",
            UserName = "root",
            Title = "v1.0.0",
        };

        var issue = new Issue();
        issue.IssueAssignees.Add(new IssueAssignee { AssigneeUserName = "user1" });
        milestone.Issues.Add(issue);

        // When
        var formatted = milestone.Format();

        // Then
        Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [Long-Long-Long-Long-Long-Long-...], [user1]", formatted);
    }

    [Fact]
    public void Should_Return_Formatted_Milestone_With_New_Line()
    {
        // Given
        var milestone = new Milestone
        {
            Description = $"Imprement xxx feature.{Environment.NewLine}Next",
            DueDate = new DateTime(2019, 1, 15),
            RepositoryName = "test",
            UserName = "root",
            Title = "v1.0.0",
        };

        var issue = new Issue();
        issue.IssueAssignees.Add(new IssueAssignee { AssigneeUserName = "user1" });
        milestone.Issues.Add(issue);

        // When
        var formatted = milestone.Format();

        // Then
        Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [Imprement xxx feature. Next], [user1]", formatted);
    }

    [Fact]
    public void Should_Return_Formatted_Milestone_With_No_Issue()
    {
        // Given
        var milestone = new Milestone
        {
            Description = "Imprement xxx feature.",
            DueDate = new DateTime(2019, 1, 15),
            RepositoryName = "test",
            UserName = "root",
            Title = "v1.0.0",
        };

        // When
        var formatted = milestone.Format();

        // Then
        Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [Imprement xxx feature.], []", formatted);
    }

    [Theory]
    [InlineData("")]
    public void Should_Return_Formatted_Milestone_With_No_AssignedUserName(string assignedUserName)
    {
        // Given
        var milestone = new Milestone
        {
            Description = "Imprement xxx feature.",
            DueDate = new DateTime(2019, 1, 15),
            RepositoryName = "test",
            UserName = "root",
            Title = "v1.0.0",
        };

        var issue = new Issue();
        issue.IssueAssignees.Add(new IssueAssignee { AssigneeUserName = assignedUserName });
        milestone.Issues.Add(issue);

        // When
        var formatted = milestone.Format();

        // Then
        Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [Imprement xxx feature.], []", formatted);
    }

    [Theory]
    [InlineData("")]
    public void Should_Return_Formatted_Milestone_With_No_Description(string description)
    {
        // Given
        var milestone = new Milestone
        {
            Description = description,
            DueDate = new DateTime(2019, 1, 15),
            RepositoryName = "test",
            UserName = "root",
            Title = "v1.0.0",
        };

        var issue = new Issue();
        issue.IssueAssignees.Add(new IssueAssignee { AssigneeUserName = "user1" });
        milestone.Issues.Add(issue);

        // When
        var formatted = milestone.Format();

        // Then
        Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [], [user1]", formatted);
    }

    [Fact]
    public void Should_Return_Formatted_Milestone_With_No_DueDate()
    {
        // Given
        var milestone = new Milestone
        {
            Description = "Imprement xxx feature.",
            DueDate = null,
            RepositoryName = "test",
            UserName = "root",
            Title = "v1.0.0",
        };

        var issue = new Issue();
        issue.IssueAssignees.Add(new IssueAssignee { AssigneeUserName = "user1" });
        milestone.Issues.Add(issue);

        // When
        var formatted = milestone.Format();

        // Then
        Assert.Equal("* [root/test], [v1.0.0], [], [Imprement xxx feature.], [user1]", formatted);
    }
}
