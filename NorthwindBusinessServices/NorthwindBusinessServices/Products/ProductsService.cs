using System;
using System.Linq;
using System.Linq.Expressions;
using NorthwindBusinessServices.Core;
using NorthwindDataAccess.Core;
using NorthwindDataAccess.Model;

namespace NorthwindBusinessServices.Products
{
    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private Expression<Func<Product, ProductDetails>> ProjectProductDetails
        {
            get
            {
                return (e) => new ProductDetails
                {
                    Id = e.ProductID,
                    Name = e.ProductName,
                    CategoryId = e.CategoryID,
                    CategoryName = e.Category.CategoryName,
                    SupplierId = e.SupplierID,
                    SupplierName = e.Supplier.CompanyName,
                    UnitPrice = e.UnitPrice,
                    UnitsInStock = e.UnitsInStock
                };
            }
        }

        public QueryResult<ProductDetails> GetProducts(int entriesPerPage, int page, GetProductsFilter filter = null)
        {
            var result = new QueryResult<ProductDetails>
            {
                Page = page,
                PageSize = entriesPerPage
            };

            var products = _unitOfWork.GetRepository<Product>().GetAll();

            var filteredProducts = filter != null ? filter.Apply(products) : products;

            var productDetails = filteredProducts.Select(ProjectProductDetails);

            result.Total = productDetails.Count();

            result.Data = productDetails
                .OrderBy(e => e.Id)
                .Page(entriesPerPage, page).ToList();

            return result;
        }

        public int CreateProduct(string productName, int supplierId, int categoryId, string quantityPerUnit,
            decimal unitPrice, short unitsInStock, short unitsOnOrder, short reorderLevel, bool deleted)
        {
            var newProduct = new Product
            {
                ProductName = productName,
                SupplierID = supplierId,
                CategoryID = categoryId,
                QuantityPerUnit = quantityPerUnit,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock,
                UnitsOnOrder = unitsOnOrder,
                ReorderLevel = reorderLevel,
                Discontinued = deleted
            };

            _unitOfWork.GetRepository<Product>().Add(newProduct);

            _unitOfWork.SaveChanges();

            return newProduct.ProductID;
        }
    }
}