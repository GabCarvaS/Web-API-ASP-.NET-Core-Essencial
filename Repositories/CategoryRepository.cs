using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<PagedList<Category>> GetCategories(CategoriesParameters categoriesParameters)
        {
            var categoriesList = await GetAll();
            var categoriesQuery = categoriesList.OrderBy(p => p.CategoryId).AsQueryable();

            var ordenedCategories = PagedList<Category>.ToPagedList(categoriesQuery, categoriesParameters.PageNumber, categoriesParameters.PageSize);
            return ordenedCategories;
        }

        public async Task<PagedList<Category>> GetCategoriesFiltredByName(CategoriesNameFilter categoriesParameters)
        {
            var categoriesList = await GetAll();
            var categoriesQuery = categoriesList.OrderBy(p => p.CategoryId).AsQueryable();

            if (!string.IsNullOrEmpty(categoriesParameters.Name))
            {
                categoriesQuery = categoriesQuery.Where(x => x.Name.Contains(categoriesParameters.Name));
            }

            var filtredCategories = PagedList<Category>.ToPagedList(categoriesQuery, categoriesParameters.PageNumber, categoriesParameters.PageSize);
            return filtredCategories;
        }
    }
}
