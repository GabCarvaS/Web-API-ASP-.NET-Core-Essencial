using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<IPagedList<Category>> GetCategories(CategoriesParameters categoriesParameters)
        {
            var categoriesList = await GetAll();
            var categoriesQuery = categoriesList.OrderBy(p => p.CategoryId).AsQueryable();

            //var ordenedCategories = PagedList<Category>.ToPagedList(categoriesQuery, categoriesParameters.PageNumber, categoriesParameters.PageSize);
            var ordenedCategories = await categoriesQuery.ToPagedListAsync(categoriesParameters.PageNumber, categoriesParameters.PageSize); // Versao mais atual 10.5.9 não permite mais o ToPagedList em memória. O GetAll() deve ser removido e a consulta deve ser feita diretamente no banco de dados.
            return ordenedCategories;
        }

        public async Task<IPagedList<Category>> GetCategoriesFiltredByName(CategoriesNameFilter categoriesParameters)
        {
            var categories = await GetAll();            

            if (!string.IsNullOrEmpty(categoriesParameters.Name))
            {
                categories = categories.Where(x => x.Name.Contains(categoriesParameters.Name));
            }

            //var filtredCategories = PagedList<Category>.ToPagedList(categoriesQuery, categoriesParameters.PageNumber, categoriesParameters.PageSize);
            var filteredCategories = await categories.ToPagedListAsync(categoriesParameters.PageNumber, categoriesParameters.PageSize); // Versao mais atual 10.5.9 não permite mais o ToPagedList em memória. O GetAll() deve ser removido e a consulta deve ser feita diretamente no banco de dados.
            return filteredCategories;
        }
    }
}
