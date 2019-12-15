using System.Threading.Tasks;
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

            // Act
            var output1 = Execute($"milestone -o {GitBucketDefaults.Owner} -r {Repository1.Name}:{Repository2.Name}");

            // Assert
            Assert.Equal($@"There are 4 open milestones.

* {Repository1.FullName}, v1.0.0, 2019/08/12, First milestone in repository1.
* {Repository1.FullName}, v1.1.0, 2019/08/13, Second milestone in repository1.
* {Repository2.FullName}, v1.0.0, 2019/08/14, First milestone in repository2.
* {Repository2.FullName}, v1.1.0, 2019/08/15, Second milestone in repository2.
", output1);
        }
    }
}
