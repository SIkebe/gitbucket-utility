using System;
using System.Threading.Tasks;
using Xunit;

namespace GbUtil.E2ETests
{
    public class CompareTest : E2ETestBase
    {
        public CompareTest(GitBucketFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_Create_PullRequest()
        {
            // Arrange
            Repository = await CreateRepository(autoInit: true);
            CreateBranch("base");
            CreateBranch("compare");
            await UpdateReadme("compare");

            // Act
            var output = Execute($"compare -o {GitBucketDefaults.Owner} --compare1 base --compare2 compare");

            // Assert
            Assert.Equal($"compare in root/{Repository.Name} has commits which has not merged into base.{Environment.NewLine}", output);
        }
    }
}
