using System.Collections.Generic;

namespace NorthwindBusinessServices.Core
{
    public class QueryResult<T> where T : class
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public ICollection<T> Data { get; set; }
    }
}