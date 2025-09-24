using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IPagedList<Category>> GetCategories(CategoriesParameters categoriesParameters);
        Task<IPagedList<Category>> GetCategoriesFiltredByName(CategoriesNameFilter categoriesParameters);
    }
}
