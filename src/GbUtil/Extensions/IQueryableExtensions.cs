using System.Linq.Expressions;

namespace System.Linq
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TSource> WhereIf<TSource>(
            this IQueryable<TSource> source,
            bool condition,
            Expression<Func<TSource, bool>> predicate)
            => condition ? source.Where(predicate) : source;
    }
}