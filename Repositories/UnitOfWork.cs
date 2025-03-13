using APICatalogo.Context;
using APICatalogo.Interfaces;

namespace APICatalogo.Repositories
{
    public class UnitOfWork : IUnitOfWork

    {
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        
        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository { get { return _productRepository ?? new ProductRepository(_context); } }
        public ICategoryRepository CategoryRepository { get { return _categoryRepository ?? new CategoryRepository(_context); } }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Dispose() 
        { 
            await _context.DisposeAsync();
        }
    }
}
