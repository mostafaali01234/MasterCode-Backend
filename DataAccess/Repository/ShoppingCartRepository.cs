using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;

namespace DataAccess.Repository
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ShoppingCart shoppingCart)
        {
            var ShoppingCartInDb = _context.ShoppingCart.FirstOrDefault(x => x.Id == shoppingCart.Id);
            if (ShoppingCartInDb != null)
            {
                ShoppingCartInDb.Price = shoppingCart.Price;
                ShoppingCartInDb.ProductId = shoppingCart.ProductId;
            }
        }
    }
}
