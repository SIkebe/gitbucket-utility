using System;
using System.Collections.Generic;
using System.Linq;
using GitBucket.Core;
using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GitBucket.Data.Repositories
{
    public abstract class LabelRepositoryBase : BaseRepository<Label>
    {
        public LabelRepositoryBase(DbContext context) : base(context)
        {
        }
    }

    public class LabelRepository : LabelRepositoryBase
    {
        public LabelRepository(DbContext context) : base(context)
        {
        }
    }
}