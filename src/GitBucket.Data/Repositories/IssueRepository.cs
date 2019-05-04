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

        public abstract Task<List<Issue>> FindIssuesRelatedToMileStone(ReleaseOptions options);
        public abstract IEnumerable<IssueLabel> FindIssueLabels(ReleaseOptions options, IEnumerable<Issue> issues);
    }

    public class IssueRepository : IssueRepositoryBase
    {
        public IssueRepository(DbContext context) : base(context)
        {
        }

        public override IEnumerable<IssueLabel> FindIssueLabels(ReleaseOptions options, IEnumerable<Issue> issues)
        {
            return Context.Set<IssueLabel>()
                .Where(l => l.UserName.Equals(options.Owner, StringComparison.OrdinalIgnoreCase))
                .Where(l => l.RepositoryName.Equals(options.Repository, StringComparison.OrdinalIgnoreCase))
                .Where(l => issues.Select(i => i.IssueId).Contains(l.IssueId));
        }

        public async override Task<List<Issue>> FindIssuesRelatedToMileStone(ReleaseOptions options)
        {
            return await Context.Set<Issue>()
                .Where(i => i.UserName.Equals(options.Owner, StringComparison.OrdinalIgnoreCase))
                .Where(i => i.RepositoryName.Equals(options.Repository, StringComparison.OrdinalIgnoreCase))
                .Where(i => i.Milestone.Title.Equals(options.MileStone, StringComparison.OrdinalIgnoreCase))
                .Where(i => i.PullRequest == options.FromPullRequest)
                .Include(i => i.Milestone)
                .Include(i => i.Priority)
                .ToListAsync();
        }
    }
}