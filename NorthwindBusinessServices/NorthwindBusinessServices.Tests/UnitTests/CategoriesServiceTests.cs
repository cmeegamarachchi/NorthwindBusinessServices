using System.Collections.Generic;
using System.Linq;
using Moq;
using NorthwindBusinessServices.Categories;
using NorthwindDataAccess.Core;
using NorthwindDataAccess.Model;
using NUnit.Framework;

namespace NorthwindBusinessServices.Tests.UnitTests
{
    [TestFixture]
    public class CategoriesServiceTests
    {
        [Test]
        public void GetCategoryDescriptions_Returns_all_categories()
        {
            // given categories
            var categoryList = new List<Category>();

            for (int i = 0; i < 100; i++)
            {
                var category = new Category
                {
                    CategoryID = i,
                    CategoryName = $"Category {i}"
                };

                categoryList.Add(category);
            }

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedCategoryRepository = new Mock<IRepository<Category>>();

            mockedUnitOfWork.Setup(e => e.GetRepository<Category>()).Returns(mockedCategoryRepository.Object);
            mockedCategoryRepository.Setup(e => e.GetAll()).Returns(categoryList.AsQueryable());

            // when GetCategoryDescriptions is called
            var categoryService = new CategoryService(mockedUnitOfWork.Object);
            var categories = categoryService.GetCategoryDescriptions();

            // then all categories are returned
            Assert.IsTrue(categories.Count() == 100);
        }
    }
}
