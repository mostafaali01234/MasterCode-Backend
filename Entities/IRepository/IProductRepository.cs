using Entities.Models;

namespace Entities.IRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void Update(Product product);
    }
}
