using Octokit;
using Xunit;
using Xunit.Abstractions;

namespace GbUtil.E2ETests;

public class MilestoneTest(GitBucketFixture fixture, ITestOutputHelper output) : E2ETestBase(fixture, output)
{
    [Fact]
    public async Task Should_Outout_Milestones()
    {
        // Arrange
        var repository1 = await CreateRepository(autoInit: true);
        var repository2 = await CreateRepository(autoInit: true);
        var client = GitBucketFixture.GitBucketClient;
        var date1 = new DateTime(2019, 8, 12);
        var date2 = new DateTime(2019, 8, 13);
        var date3 = new DateTime(2019, 8, 14);
        var date4 = new DateTime(2019, 8, 15);
        var date1Utc = DateTime.SpecifyKind(date1, DateTimeKind.Utc);
        var date2Utc = DateTime.SpecifyKind(date2, DateTimeKind.Utc);
        var date3Utc = DateTime.SpecifyKind(date3, DateTimeKind.Utc);
        var date4Utc = DateTime.SpecifyKind(date4, DateTimeKind.Utc);
        var milestone11 = await client.Issue.Milestone.Create(repository1.Owner.Login, repository1.Name, new NewMilestone("v1.0.0") { Description = "First milestone in repository1.", DueOn = date1Utc, });
        var milestone12 = await client.Issue.Milestone.Create(repository1.Owner.Login, repository1.Name, new NewMilestone("v1.1.0") { Description = "Second milestone in repository1.", DueOn = date2Utc });
        var milestone21 = await client.Issue.Milestone.Create(repository2.Owner.Login, repository2.Name, new NewMilestone("v1.0.0") { Description = "First milestone in repository2.", DueOn = date3Utc });
        var milestone22 = await client.Issue.Milestone.Create(repository2.Owner.Login, repository2.Name, new NewMilestone("v1.1.0") { Description = "Second milestone in repository2.", DueOn = date4Utc });

        var newIssue11 = new NewIssue("Bump to v1.0.0") { Milestone = milestone11.Number };
        newIssue11.Assignees.Add("user1");
        await client.Issue.Create(GitBucketDefaults.Owner, repository1.Name, newIssue11);

        var newIssue12 = new NewIssue("Bump to v1.1.0") { Milestone = milestone12.Number };
        newIssue12.Assignees.Add("user1");
        await client.Issue.Create(GitBucketDefaults.Owner, repository1.Name, newIssue12);

        var newIssue13 = new NewIssue("Another one") { Milestone = milestone12.Number };
        newIssue13.Assignees.Add("user2");
        await client.Issue.Create(GitBucketDefaults.Owner, repository1.Name, newIssue13);

        var newIssue21 = new NewIssue("Bump to v1.0.0") { Milestone = milestone21.Number };
        newIssue21.Assignees.Add(GitBucketDefaults.Owner);
        await client.Issue.Create(GitBucketDefaults.Owner, repository2.Name, newIssue21);

        var newIssue22 = new NewIssue("Bump to v1.1.0") { Milestone = milestone22.Number };
        await client.Issue.Create(GitBucketDefaults.Owner, repository2.Name, newIssue22);

        // Act
        var output1 = Execute($"milestone -o {GitBucketDefaults.Owner} -r {repository1.Name}:{repository2.Name}");
        Output.WriteLine("test log output");
        Output.WriteLine(output1);

        // Assert
        Assert.Equal($@"There are 4 open milestones.

* [{repository1.FullName}], [v1.0.0], [2019/08/12], [First milestone in repository1...], [user1]
* [{repository1.FullName}], [v1.1.0], [2019/08/13], [Second milestone in repository...], [user1, user2]
* [{repository2.FullName}], [v1.0.0], [2019/08/14], [First milestone in repository2...], [root]
* [{repository2.FullName}], [v1.1.0], [2019/08/15], [Second milestone in repository...], []
",
            output1,
            ignoreLineEndingDifferences: true);
    }
}
