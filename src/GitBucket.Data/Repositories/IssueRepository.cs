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
        public abstract Task<List<IssueLabel>> FindIssueLabels(ReleaseOptions options, IEnumerable<Issue> issues);
    }

    public class IssueRepository : IssueRepositoryBase
    {
        public IssueRepository(DbContext context) : base(context)
        {
        }

        public async override Task<List<IssueLabel>> FindIssueLabels(ReleaseOptions options, IEnumerable<Issue> issues)
        {
#pragma warning disable CA1304 // Specify CultureInfo
            // "String.Equals(String, StringComparison)" causes client side evaluation.
            // https://github.com/aspnet/EntityFrameworkCore/issues/1222
            return await Context.Set<IssueLabel>()
                .Where(l => l.UserName.ToLower() == options.Owner.ToLower())
                .Where(l => l.RepositoryName.ToLower() == options.Repository.ToLower())
                .Where(l => issues.Select(i => i.IssueId).Contains(l.IssueId))
                .AsNoTracking()
                .ToListAsync();
        }

        public async override Task<List<Issue>> FindIssuesRelatedToMileStone(ReleaseOptions options)
        {
            // "String.Equals(String, StringComparison)" causes client side evaluation.
            // https://github.com/aspnet/EntityFrameworkCore/issues/1222
            return await Context.Set<Issue>()
                .Where(i => i.UserName.ToLower() == options.Owner.ToLower())
                .Where(i => i.RepositoryName.ToLower() == options.Repository.ToLower())
                .Where(i => i.Milestone.Title.ToLower() == options.MileStone.ToLower())
                .Where(i => i.PullRequest == options.FromPullRequest)
                .Include(i => i.Milestone)
                .Include(i => i.Priority)
                .AsNoTracking()
                .ToListAsync();
#pragma warning restore CA1304 // Specify CultureInfo
        }
    }
}