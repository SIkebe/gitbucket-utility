using System.Linq;
using Octokit;
using Xunit;

namespace GbUtil.E2ETests
{
    public class UnitTest1 : IClassFixture<GitBucketFixture>
    {
        private readonly GitBucketFixture _fixture;

        public UnitTest1(GitBucketFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void Should_Create_PullRequest()
        {
            var contents = await _fixture.GitBucketClient.Repository.Content.GetAllContents(GitBucketDefaults.Owner, GitBucketDefaults.Repository1);
            var readme = contents.Where(c => c.Name == "README.md").FirstOrDefault();
            await _fixture.GitBucketClient.Repository.Content.UpdateFile(
                GitBucketDefaults.Owner,
                GitBucketDefaults.Repository1,
                "README.md",
                new UpdateFileRequest("commit message", "new file content", readme.Sha));
        }
    }
}
