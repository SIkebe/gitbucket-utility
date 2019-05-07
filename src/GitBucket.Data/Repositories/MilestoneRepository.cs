using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitBucket.Core;
using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GitBucket.Data.Repositories
{
    public abstract class MilestoneRepositoryBase : BaseRepository<Milestone>
    {
        public MilestoneRepositoryBase(DbContext context) : base(context)
        {
        }

        public abstract Task<List<Milestone>> FindMilestones(MilestoneOptions options);
    }

    public class MilestoneRepository : MilestoneRepositoryBase
    {
        public MilestoneRepository(DbContext context) : base(context)
        {
        }

        public override async Task<List<Milestone>> FindMilestones(MilestoneOptions options)
        {
#pragma warning disable CA1304 // Specify CultureInfo
            // "String.Equals(String, StringComparison)" causes client side evaluation.
            // https://github.com/aspnet/EntityFrameworkCore/issues/1222
            var owners = options.Owners.Select(o => o.ToLower());
            var repositories = options.Repositories.Select(r => r.ToLower());

            return await Context.Set<Milestone>()
                .WhereIf(options.Owners.Any(), m => owners.Contains(m.UserName.ToLower()))
                .WhereIf(options.Repositories.Any(), m => repositories.Contains(m.RepositoryName.ToLower()))
                .WhereIf(!options.IncludeClosed, m => m.ClosedDate == null)
                .OrderBy(m => m.DueDate)
                .ThenBy(m => m.UserName)
                .ThenBy(m => m.RepositoryName)
                .AsNoTracking()
                .ToListAsync();
#pragma warning restore CA1304 // Specify CultureInfo
        }
    }
}