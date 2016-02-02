using NorthwindBusinessServices.Core;

namespace NorthwindBusinessServices.Products
{
    public interface IProductsService
    {
        QueryResult<ProductDetails> GetProducts(int entriesPerPage, int page, GetProductsFilter filter = null);

        int CreateProduct(string productName, int supplierId, int categoryId, string quantityPerUnit,
            decimal unitPrice, short unitsInStock, short unitsOnOrder, short reorderLevel, bool deleted);
    }
}