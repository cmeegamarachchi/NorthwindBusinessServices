using System.Collections.Generic;

namespace NorthwindBusinessServices.Categories
{
    public interface ICategoryService
    {
        ICollection<CategoryDescription> GetCategoryDescriptions();
    }
}