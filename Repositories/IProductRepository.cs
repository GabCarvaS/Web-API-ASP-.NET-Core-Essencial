using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProducts();
        Task<Product> GetProduct(int id);
        Task<Product> Create(Product product);
        Task<bool> Update(Product product);
        Task<bool> Delete(int id);
    }
}
