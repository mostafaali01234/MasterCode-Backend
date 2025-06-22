using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;
using Utilities;

namespace DataAccess.Repository
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderHeader orderHeader)
        {
            var orderHeaderInDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == orderHeader.Id);
            if (orderHeaderInDb != null)
            {
                orderHeaderInDb.OrderDate = orderHeader.OrderDate;
                orderHeaderInDb.OrderTotal = orderHeader.OrderTotal;
                orderHeaderInDb.OrderStatus = orderHeader.OrderStatus;
                orderHeaderInDb.OrderNotes = orderHeader.OrderNotes;
                orderHeaderInDb.CustomerId = orderHeader.CustomerId;
                orderHeaderInDb.TechId = orderHeader.TechId;
                orderHeaderInDb.InstallDate = orderHeader.InstallDate;
                orderHeaderInDb.PaymentStatus = orderHeader.PaymentStatus;
                orderHeaderInDb.ApplicationUserId = orderHeader.ApplicationUserId;
            }
        }

        public void CancelOrder(OrderHeader orderHeader)
        {
            var orderHeaderInDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == orderHeader.Id);
            if (orderHeaderInDb != null)
            {
                orderHeaderInDb.OrderStatus = SD.StatusCanceled;
            }
        }
        public void CompleteOrder(OrderHeader orderHeader)
        {
            var orderHeaderInDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == orderHeader.Id);
            if (orderHeaderInDb != null)
            {
                orderHeaderInDb.OrderStatus = SD.StatusDone;
                orderHeaderInDb.InstallDate = orderHeader.InstallDate;
            }
        }
    }
}
