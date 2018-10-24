using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public abstract Task<List<Issue>> FindIssuesRelatedToMileStone(ReleaseOptions option);
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
                .Where(l => l.UserName.Equals(option.Owner, StringComparison.OrdinalIgnoreCase))
                .Where(l => l.RepositoryName.Equals(option.Repository, StringComparison.OrdinalIgnoreCase))
                .Where(l => issues.Select(i => i.IssueId).Contains(l.IssueId));
        }

        public async override Task<List<Issue>> FindIssuesRelatedToMileStone(ReleaseOptions option)
        {
            return await Context.Set<Issue>()
                .Where(i => i.UserName.Equals(option.Owner, StringComparison.OrdinalIgnoreCase))
                .Where(i => i.RepositoryName.Equals(option.Repository, StringComparison.OrdinalIgnoreCase))
                .Where(i => i.Milestone.Title.Equals(option.MileStone, StringComparison.OrdinalIgnoreCase))
                .WhereIf(string.Equals(nameof(ReleaseNoteTarget.Issues), option.Target, StringComparison.OrdinalIgnoreCase),
                    i => !i.PullRequest)
                .WhereIf(!string.Equals(nameof(ReleaseNoteTarget.Issues), option.Target, StringComparison.OrdinalIgnoreCase),
                    i => i.PullRequest)
                .Include(i => i.Milestone)
                .Include(i => i.Priority)
                .ToListAsync();
        }
    }
}