using APICatalogo.Models;

namespace APICatalogo.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategory(int id);
    }
}
