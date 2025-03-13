using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetProductsByCategory(int id)
        {
            var products = await GetAll();
            return products.Where(x => x.CategoryId == id);
        }
    }
}
