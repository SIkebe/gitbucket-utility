using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GitBucket.Data.Repositories
{
    public interface IRepository<T>
    {
        T FindById(int id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> pridictate);
        IEnumerable<T> FindAll();
        void Add(T item);
        void Update(T item);
        void Delete(T item);
        void Save();
    }
}