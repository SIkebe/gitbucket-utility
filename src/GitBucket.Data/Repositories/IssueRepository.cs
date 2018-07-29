using System;
using System.Collections.Generic;
using System.Linq;
using GitBucket.Core;
using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GitBucket.Data.Repositories
{
    public abstract class IssueRepositoryBase : BaseRepository<Issue>
    {
        public IssueRepositoryBase(DbContext context) : base(context)
        {
        }

        public abstract IEnumerable<Issue> FindIssuesRelatedToMileStone(ReleaseOptions option);
        public abstract IEnumerable<IssueLabel> FindIssueLabels(ReleaseOptions option, IEnumerable<Issue> issues);
    }

    public class IssueRepository : IssueRepositoryBase
    {
        public IssueRepository(DbContext context) : base(context)
        {
        }

        public override IEnumerable<IssueLabel> FindIssueLabels(ReleaseOptions option, IEnumerable<Issue> issues)
        {
            return Context.Set<IssueLabel>()
                .Where(l => l.UserName == option.Owner)
                .Where(l => l.RepositoryName == option.Repository)
                .Where(l => issues.Select(i => i.IssueId).Contains(l.IssueId));
        }

        public override IEnumerable<Issue> FindIssuesRelatedToMileStone(ReleaseOptions option)
        {
            return Context.Set<Issue>()
                .Where(i => i.UserName == option.Owner)
                .Where(i => i.RepositoryName == option.Repository)
                .Where(i => i.Milestone.Title == option.MileStone)
                .WhereIf(string.Equals(nameof(ReleaseNoteTarget.Issues), option.Target, StringComparison.OrdinalIgnoreCase),
                    i => !i.PullRequest)
                .WhereIf(!string.Equals(nameof(ReleaseNoteTarget.Issues), option.Target, StringComparison.OrdinalIgnoreCase),
                    i => i.PullRequest)
                .Include(i => i.Milestone)
                .Include(i => i.Priority);
        }
    }
}