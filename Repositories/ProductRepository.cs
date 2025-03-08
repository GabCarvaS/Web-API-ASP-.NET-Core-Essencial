using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Create(Product product)
        {
            if (product is null) { throw new ArgumentNullException(nameof(Product)); }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is not null) 
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
        }

        public IQueryable<Product> GetProducts()
        {
            return _context.Products;
        }

        public async Task<bool> Update(Product product)
        {
            if (product is null) { throw new ArgumentNullException(nameof(product)); }

            if(_context.Products.Any(x => x.ProductId == product.ProductId))
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
