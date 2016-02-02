using System.Linq;
using NorthwindDataAccess.Model;

namespace NorthwindBusinessServices.Products
{
    public class GetProductsFilter
    {
        public int[] Ids { get; set; }

        public IQueryable<Product> Apply(IQueryable<Product> baseQuery)
        {
            if (Ids.Length > 0)
            {
                baseQuery = baseQuery.Where(e => Ids.Contains(e.ProductID));
            }

            return baseQuery;
        }
    }
}