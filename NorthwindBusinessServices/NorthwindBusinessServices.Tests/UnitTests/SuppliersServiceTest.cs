using System.Collections.Generic;
using System.Linq;
using Moq;
using NorthwindBusinessServices.Suppliers;
using NorthwindDataAccess.Core;
using NorthwindDataAccess.Model;
using NUnit.Framework;

namespace NorthwindBusinessServices.Tests.UnitTests
{
    [TestFixture]
    public class SuppliersServiceTest
    {
        [Test]
        public void GetSupplierDescriptions_returns_all_supplier_descriptions()
        {
            // Given 100 suppliers
            var supplierList = new List<Supplier>();

            for (var i = 0; i < 100; i++)
            {
                var supplier = new Supplier
                {
                    SupplierID = i,
                    CompanyName = $"Supplier {i}"
                };

                supplierList.Add(supplier);
            }

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedSupplierRepository = new Mock<IRepository<Supplier>>();

            mockedUnitOfWork.Setup(e => e.GetRepository<Supplier>()).Returns(mockedSupplierRepository.Object);
            mockedSupplierRepository.Setup(e => e.GetAll()).Returns(supplierList.AsQueryable());

            // When GetSupplierDescriptions is called
            var supplierService = new SuppliersService(mockedUnitOfWork.Object);
            var suppliers = supplierService.GetSupplierDescriptions();

            // Then all 100 SupplierDescriptions are returned
            Assert.IsTrue(suppliers.Count() == 100);
        }
    }
}