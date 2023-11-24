using Octokit;
namespace GitBucket.Service.Tests;

public sealed class FakePullRequest(GitReference head, GitReference @base, int number = 1) : Octokit.PullRequest(0, null, null, null, null, null, null, null, number, ItemState.Open, null, null, DateTimeOffset.Now, DateTimeOffset.Now, null, null, head, @base, null, null, null, false, null, null, null, null, 0, 0, 0, 0, 0, null, false, null, null, null, null, null)
{
}

public sealed class FakeGitReference : Octokit.GitReference
{
    public FakeGitReference(string @ref) => Ref = @ref;
}

public sealed class FakeRepository(User owner, string repository, DateTimeOffset createdAt) : Octokit.Repository(null, null, null, null, null, null, null, null, 0, null, owner, repository, owner?.Login + "/" + repository, false, null, null, null, false, false, 0, 0, null, 0, null, createdAt, createdAt, null, null, null, null, false, false, false, false, false, 0, 0, false, false, false, false, 0, false, RepositoryVisibility.Public, Array.Empty<string>(), false, false, false)
{
}
