using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderDetail orderDetail)
        {
            var orderDetailInDb = _context.OrderDetails.FirstOrDefault(x => x.Id == orderDetail.Id);
            if (orderDetailInDb != null)
            {
                orderDetailInDb.Price = orderDetail.Price;
                orderDetailInDb.OrderHeaderId = orderDetail.OrderHeaderId;
                orderDetailInDb.ProductId = orderDetail.ProductId;
            }
        }
    }
}
