using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategory(int id);
        Task<PagedList<Product>> GetProducts(ProductsParameters producstsParameters);
        Task<PagedList<Product>> GetProductsFiltredByPrice(ProductsPriceFilter productsPriceFilterParameters);
    }
}
