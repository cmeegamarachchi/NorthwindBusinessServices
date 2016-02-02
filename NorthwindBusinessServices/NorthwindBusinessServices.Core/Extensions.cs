using System.Linq;

namespace NorthwindBusinessServices.Core
{
    public static class Extensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> orderedQuery, int pageSize, int page)
        {
            var skip = page > 0 ? (page - 1)* pageSize : 0;

            return orderedQuery.Skip(skip).Take(pageSize);
        }
    }
}
