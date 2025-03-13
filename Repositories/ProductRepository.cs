using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<PagedList<Product>> GetProducts(ProductsParameters producstsParameters)
        {
            var productsList = await GetAll();
            var productsQuery = productsList.OrderBy(p => p.ProductId).AsQueryable();

            var ordenetProducts = PagedList<Product>.ToPagedList(productsQuery, producstsParameters.PageNumber, producstsParameters.PageSize);
            return ordenetProducts;
        }

        //public async Task<IEnumerable<Product>> GetProducts(ProductsParameters producstsParameters)
        //{
        //    var products = await GetAll();
        //    return products
        //        .OrderBy(x => x.Name)
        //        .Skip((producstsParameters.PageNumber - 1) * producstsParameters.PageSize)
        //        .Take(producstsParameters.PageSize) .ToList();
        //}

        public async Task<IEnumerable<Product>> GetProductsByCategory(int id)
        {
            var products = await GetAll();
            return products.Where(x => x.CategoryId == id);
        }
    }
}
