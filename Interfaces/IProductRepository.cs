using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategory(int id);
        Task<IPagedList<Product>> GetProducts(ProductsParameters producstsParameters);
        Task<IPagedList<Product>> GetProductsFiltredByPrice(ProductsPriceFilter productsPriceFilterParameters);
    }
}
