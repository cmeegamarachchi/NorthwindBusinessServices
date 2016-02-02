using System.Collections.Generic;
using System.Linq;
using NorthwindDataAccess.Core;
using NorthwindDataAccess.Model;

namespace NorthwindBusinessServices.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;

        public CategoryService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ICollection<CategoryDescription> GetCategoryDescriptions()
        {
            return _uow.GetRepository<Category>().GetAll().Select(e => new CategoryDescription
            {
                Id = e.CategoryID,
                Name = e.CategoryName
            }).ToList();
        }
    }
}