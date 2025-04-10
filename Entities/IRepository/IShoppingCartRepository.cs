using Entities.Models;

namespace Entities.IRepository
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        void Update(ShoppingCart shoppingCart);
    }
}
