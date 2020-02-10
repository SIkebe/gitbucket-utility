using Octokit;
using System;

namespace GitBucket.Service.Tests
{
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

    public sealed class FakeRepository : Octokit.Repository
    {
        public FakeRepository(User owner, string repository, DateTimeOffset createdAt)
        {
            Owner = owner;
            Name = repository;
            FullName = owner?.Login + "/" + repository;
            CreatedAt = createdAt;
        }
    }
}
