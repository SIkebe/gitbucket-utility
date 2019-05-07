using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace GitBucket.Data.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class
    {
        public BaseRepository(DbContext context) => Context = context;

        protected DbContext Context { get; set; }

        public virtual TEntity FindById(int id) => Context.Set<TEntity>().Find(id);

        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predictate)
            => Context.Set<TEntity>()
            .Where(predictate)
            .AsNoTracking();

        public virtual IEnumerable<TEntity> FindAll() => Context.Set<TEntity>().AsEnumerable();

        public virtual void Add(TEntity item) => Context.Set<TEntity>().Add(item);

        public virtual void Update(TEntity item)
        {
            Context.Set<TEntity>().Attach(item);
            Context.Set<TEntity>().Update(item);
        }

        public virtual void AddRange(IEnumerable<TEntity> drives) => Context.Set<TEntity>().AddRange(drives);

        public virtual void Delete(TEntity item)
        {
            Context.Set<TEntity>().Attach(item);
            Context.Set<TEntity>().Remove(item);
        }

        public virtual void Save() => Context.SaveChanges();

#pragma warning disable SA1201 // Elements must appear in the correct order
        private bool disposedValue = false; // To detect redundant calls
#pragma warning restore SA1201 // Elements must appear in the correct order

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        Context.Dispose();
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch { }
#pragma warning restore CA1031 // Do not catch general exception types
                }

                disposedValue = true;
            }
        }

#pragma warning disable SA1202 // Elements must be ordered by access
        public void Dispose()
#pragma warning restore SA1202 // Elements must be ordered by access
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
