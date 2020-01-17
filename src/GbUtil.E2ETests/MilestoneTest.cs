using System.Threading.Tasks;
using Octokit;
using Xunit;

namespace GbUtil.E2ETests
{
    public class MilestoneTest : E2ETestBase
    {
        public MilestoneTest(GitBucketFixture fixture) : base(fixture)
        {
        }

        public Octokit.Repository Repository1 { get; set; } = default!;
        public Octokit.Repository Repository2 { get; set; } = default!;

        [Fact]
        public async Task Should_Outout_Milestones()
        {
            // Arrange
            Repository1 = await CreateRepository(autoInit: true);
            Repository2 = await CreateRepository(autoInit: true);
            GitBucketFixture.CreateMilestone(Repository1, "v1.0.0", "First milestone in repository1.", "2019-08-12");
            GitBucketFixture.CreateMilestone(Repository1, "v1.1.0", "Second milestone in repository1.", "2019-08-13");
            GitBucketFixture.CreateMilestone(Repository2, "v1.0.0", "First milestone in repository2.", "2019-08-14");
            GitBucketFixture.CreateMilestone(Repository2, "v1.1.0", "Second milestone in repository2.", "2019-08-15");

            var newIssue11 = new NewIssue("Bump to v1.0.0");
            newIssue11.Assignees.Add("user1");
            var issue11 = await GitBucketFixture.GitBucketClient.Issue.Create(GitBucketDefaults.Owner, Repository1.Name, newIssue11);
            SetMilestone(issue11, Repository1, "v1.0.0");

            var newIssue12 = new NewIssue("Bump to v1.1.0");
            newIssue12.Assignees.Add("user1");
            var issue12 = await GitBucketFixture.GitBucketClient.Issue.Create(GitBucketDefaults.Owner, Repository1.Name, newIssue12);
            SetMilestone(issue12, Repository1, "v1.1.0");

            var newIssue13 = new NewIssue("Another one");
            newIssue13.Assignees.Add("user2");
            var issue13 = await GitBucketFixture.GitBucketClient.Issue.Create(GitBucketDefaults.Owner, Repository1.Name, newIssue13);
            SetMilestone(issue13, Repository1, "v1.1.0");

            var newIssue21 = new NewIssue("Bump to v1.0.0");
            newIssue21.Assignees.Add(GitBucketDefaults.Owner);
            var issue21 = await GitBucketFixture.GitBucketClient.Issue.Create(GitBucketDefaults.Owner, Repository2.Name, newIssue21);
            SetMilestone(issue21, Repository2, "v1.0.0");

            var newIssue22 = new NewIssue("Bump to v1.1.0");
            var issue22 = await GitBucketFixture.GitBucketClient.Issue.Create(GitBucketDefaults.Owner, Repository2.Name, newIssue22);
            SetMilestone(issue22, Repository2, "v1.1.0");

            // Act
            var output1 = Execute($"milestone -o {GitBucketDefaults.Owner} -r {Repository1.Name}:{Repository2.Name}");

            // Assert
            Assert.Equal($@"There are 4 open milestones.

* [{Repository1.FullName}], [v1.0.0], [2019/08/12], [First milestone in repository1...], [user1]
* [{Repository1.FullName}], [v1.1.0], [2019/08/13], [Second milestone in repository...], [user1, user2]
* [{Repository2.FullName}], [v1.0.0], [2019/08/14], [First milestone in repository2...], [root]
* [{Repository2.FullName}], [v1.1.0], [2019/08/15], [Second milestone in repository...], []
", output1);
        }
    }
}
