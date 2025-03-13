using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<PagedList<Category>> GetCategories(CategoriesParameters categoriesParameters);
        Task<PagedList<Category>> GetCategoriesFiltredByName(CategoriesNameFilter categoriesParameters);
    }
}
