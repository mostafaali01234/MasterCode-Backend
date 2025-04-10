using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;

namespace DataAccess.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var ProductInDb = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (ProductInDb != null)
            {
                ProductInDb.Title = product.Title;
                ProductInDb.Description = product.Description;
                ProductInDb.Price = product.Price;
                ProductInDb.CategoryId = product.CategoryId;
                ProductInDb.ImageUrl = product.ImageUrl;
            }
        }
    }
}
