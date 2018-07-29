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
            => Context.Set<TEntity>().Where(predictate);

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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

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
                    catch { }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
