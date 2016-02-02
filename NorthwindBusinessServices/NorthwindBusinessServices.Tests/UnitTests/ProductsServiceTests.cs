using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NorthwindBusinessServices.Products;
using NorthwindDataAccess.Core;
using NorthwindDataAccess.Model;
using NUnit.Framework;

namespace NorthwindBusinessServices.Tests.UnitTests
{
    [TestFixture]
    public class ProductsServiceTests
    {
        [Test]
        public void GetProducts_Returns_ProductDescriptions()
        {
            var entriesPerPage = 5;
            var page = 2;

            // given there are products with suppliers and categories
            var supplier = new Supplier { SupplierID = 1, CompanyName = "Supplier 1" };
            var category = new Category { CategoryID = 1, CategoryName = "Category 1" };
            var products = new List<Product>();

            for (var i = 0; i < 100; i++)
            {
                var product = new Product
                {
                    ProductID = i,
                    ProductName = $"Product {i}",
                    Category = category,
                    Supplier = supplier
                };

                products.Add(product);
            }

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedProductRepository = new Mock<IRepository<Product>>();

            mockedUnitOfWork.Setup(e => e.GetRepository<Product>()).Returns(mockedProductRepository.Object);
            mockedProductRepository.Setup(e => e.GetAll()).Returns(products.AsQueryable());

            // when GetProducts is called
            var productsService = new ProductsService(mockedUnitOfWork.Object);
            var productDescriptions = productsService.GetProducts(entriesPerPage, page).Data;

            // then correct set of ProductDescriptions are returned
            Assert.IsTrue(productDescriptions.Count() == 5, "Descriptions for 5 products are returned");
            Assert.IsTrue(productDescriptions.Select(e => e.Id).Intersect(new [] { 5, 6, 7, 8, 9 }).Count() == 5, "Products in page 2 are returned");
        }


        [Test]
        public void GetProducts_When_Filtered_By_Ids_Returns_CorrectSet()
        {
            // given there are products with suppliers and categories
            var supplier = new Supplier { SupplierID = 1, CompanyName = "Supplier 1" };
            var category = new Category { CategoryID = 1, CategoryName = "Category 1" };
            var products = new List<Product>();

            for (var i = 0; i < 100; i++)
            {
                var product = new Product
                {
                    ProductID = i,
                    ProductName = $"Product {i}",
                    Category = category,
                    Supplier = supplier
                };

                products.Add(product);
            }

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedProductRepository = new Mock<IRepository<Product>>();

            mockedUnitOfWork.Setup(e => e.GetRepository<Product>()).Returns(mockedProductRepository.Object);
            mockedProductRepository.Setup(e => e.GetAll()).Returns(products.AsQueryable());

            // when GetProducts is called with id filter
            var productsService = new ProductsService(mockedUnitOfWork.Object);
            var getProductsFilter = new GetProductsFilter {Ids = new[] {1, 7, 15, 22, 9}};
            var productDescriptions = productsService.GetProducts(Int32.MaxValue, 1, getProductsFilter).Data;

            // then correct set of product descriptions are returned
            Assert.IsTrue(productDescriptions.Select(e => e.Id).Intersect(new[] { 1, 7, 15, 22, 9 }).Count() == 5, "Products with ids 1, 7, 15, 22, 9 are returned");
        }

        [Test]
        public void CreateProduct_Adds_new_product()
        {
            // given product details
            var products = new List<Product>();
            var productName = "Latest product";
            var supplierId = 1;
            var categoryId = 2;
            var quantityPerUnit = "test";
            var unitPrice = 1.22m;
            short unitsInStock = 2;
            short unitsOnOrder = 3;
            short reorderLevel = 4;
            
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedProductRepository = new Mock<IRepository<Product>>();

            mockedUnitOfWork.Setup(e => e.GetRepository<Product>()).Returns(mockedProductRepository.Object);
            mockedUnitOfWork.Setup(e => e.SaveChanges()).Callback(() => { products.First().ProductID = 10; });
            mockedProductRepository.Setup(e => e.Add(It.IsAny<Product>())).Returns<Product>(p => { products.Add(p); return  p;});
            mockedProductRepository.Setup(e => e.GetAll()).Returns(products.AsQueryable());

            // when CreateProduct is called
            var productsService = new ProductsService(mockedUnitOfWork.Object);
            var id = productsService.CreateProduct(productName, supplierId, categoryId, quantityPerUnit, unitPrice, unitsInStock, unitsOnOrder, reorderLevel, false);

            // then new product is added
            Assert.IsTrue(products.Any());
            Assert.IsTrue(id == 10);
            Assert.IsTrue(products.First().SupplierID == supplierId);
            Assert.IsTrue(products.First().CategoryID == categoryId);
            Assert.IsTrue(products.First().QuantityPerUnit == quantityPerUnit);
            Assert.IsTrue(products.First().UnitPrice == unitPrice);
            Assert.IsTrue(products.First().UnitsInStock == unitsInStock);
            Assert.IsTrue(products.First().UnitsOnOrder == unitsOnOrder);
            Assert.IsTrue(products.First().ReorderLevel == reorderLevel);
            Assert.IsTrue(products.First().Discontinued == false);
        }
    }
}
