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
            return await Context.Set<Milestone>()
                .WhereIf(options.Owners.Any(), m => options.Owners.Contains(m.UserName, StringComparer.OrdinalIgnoreCase))
                .WhereIf(options.Repositories.Any(), m => options.Repositories.Contains(m.RepositoryName, StringComparer.OrdinalIgnoreCase))
                .WhereIf(!options.IncludeClosed, m => m.ClosedDate == null)
                .OrderBy(m => m.DueDate)
                .ThenBy(m => m.UserName)
                .ThenBy(m => m.RepositoryName)
                .ToListAsync();
        }
    }
}