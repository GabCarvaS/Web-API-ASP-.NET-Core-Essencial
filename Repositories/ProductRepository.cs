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

        public async Task<IEnumerable<Product>> GetProductsByCategory(int id)
        {
            var products = await GetAll();
            return products.Where(x => x.CategoryId == id);
        }

        public async Task<PagedList<Product>> GetProductsFiltredByPrice(ProductsPriceFilter productsPriceFilterParameters)
        {
            var productsList = await GetAll();
            var productsQuery = productsList.OrderBy(p => p.ProductId).AsQueryable();

            if(productsPriceFilterParameters.Price.HasValue && !string.IsNullOrEmpty(productsPriceFilterParameters.PriceCriterion))
            {
                if (productsPriceFilterParameters.PriceCriterion.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    productsQuery = productsQuery.Where(p => p.Price > productsPriceFilterParameters.Price.Value).OrderBy(p => p.Price);
                }
                else if (productsPriceFilterParameters.PriceCriterion.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    productsQuery = productsQuery.Where(p => p.Price < productsPriceFilterParameters.Price.Value).OrderBy(p => p.Price);
                }
                else if (productsPriceFilterParameters.PriceCriterion.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    productsQuery = productsQuery.Where(p => p.Price == productsPriceFilterParameters.Price.Value).OrderBy(p => p.Price);
                }
            }

            var filtredProducts = PagedList<Product>.ToPagedList(productsQuery, productsPriceFilterParameters.PageNumber, productsPriceFilterParameters.PageSize);
            return filtredProducts;
        }
    }
}
