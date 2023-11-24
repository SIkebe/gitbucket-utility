using LibGit2Sharp;
using Octokit;
using Xunit;
using Xunit.Abstractions;

namespace GbUtil.E2ETests;

public class PullRequestTest(GitBucketFixture fixture, ITestOutputHelper output) : E2ETestBase(fixture, output)
{
    [Fact]
    public async Task Should_Create_PullRequest()
    {
        // Arrange
        await PrepareForPR();

        // Act
        var output1 = Execute($"release -o {GitBucketDefaults.Owner} -r {Repository.Name} -m v1.0.0 --create-pr -f");
        Console.WriteLine($"output1: {output1}");

        var output2 = Execute($"release -o {GitBucketDefaults.Owner} -r {Repository.Name} -m v1.0.0 --create-pr -f");
        Console.WriteLine($"output2: {output2}");

        // Assert
        Assert.Equal($"A new pull request has been successfully created!{Environment.NewLine}", output1);
        Assert.Equal($"A pull request already exists for {GitBucketDefaults.Owner}:develop.{Environment.NewLine}", output2);

        var pr = await GitBucketFixture.GitBucketClient.PullRequest.Get(GitBucketDefaults.Owner, Repository.Name, 3);
        Assert.Equal("master", pr.Base.Ref);
        Assert.Equal("develop", pr.Head.Ref);
        Assert.Equal("v1.0.0", pr.Title);
        Assert.Equal(ItemState.Open, pr.State);
        Assert.True(Enumerable.SequenceEqual(pr.Labels.Select(l => l.Name).OrderBy(l => l), new[] { "Bug", "Enhancement" }.OrderBy(l => l)));
        Assert.Equal(@"As part of this release we had 2 issues closed.
The highest priority among them is """".

### Enhancement
* Bump to v1.0.0 #1

### Bug
* Found a bug #2

",
            pr.Body,
            ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Should_Create_Draft_PullRequest()
    {
        // Arrange
        await PrepareForPR();

        // Act
        _ = Execute($@"release -o {GitBucketDefaults.Owner} -r {Repository.Name} -m ""v1.0.0"" --title ""v1.0.0 draft"" --create-pr --draft -f");

        // Assert
        var allPrs = await GitBucketFixture.GitBucketClient.PullRequest.GetAllForRepository(GitBucketDefaults.Owner, Repository.Name);
        var pr = allPrs.Single(p => p.Title == "v1.0.0 draft");
        Assert.True(pr.Draft);
    }

    [Fact]
    public async Task Should_Output_ReleaseNote()
    {
        // Arrange
        await PrepareForPR();

        // Act
        var output = Execute($"release -o {GitBucketDefaults.Owner} -r {Repository.Name} -m v1.0.0 -f");

        // Assert
        Assert.Equal(@"As part of this release we had 2 issues closed.
The highest priority among them is """".

### Enhancement
* Bump to v1.0.0 #1

### Bug
* Found a bug #2


",
            output,
            ignoreLineEndingDifferences: true);
    }

    private async Task PrepareForPR()
    {
        // Create base and target branch
        Repository = await CreateRepository(autoInit: true);
        CreateBranch("develop");
        await UpdateReadme("develop");
        var client = GitBucketFixture.GitBucketClient;

        // Create milestone v1.0.0
        var milestone = await client.Issue.Milestone.Create(GitBucketDefaults.Owner, Repository.Name, new NewMilestone("v1.0.0"));

        // Create issues which target milestone v1.0.0
        var issue1 = await client.Issue.Create(GitBucketDefaults.Owner, Repository.Name, new NewIssue("Bump to v1.0.0") { Milestone = milestone.Number });
        await client.Issue.Labels.AddToIssue(GitBucketDefaults.Owner, Repository.Name, issue1.Number, new[] { "Enhancement" });

        var issue2 = await client.Issue.Create(GitBucketDefaults.Owner, Repository.Name, new NewIssue("Found a bug") { Milestone = milestone.Number });
        await client.Issue.Labels.AddToIssue(GitBucketDefaults.Owner, Repository.Name, issue2.Number, new[] { "Bug" });
    }
}
