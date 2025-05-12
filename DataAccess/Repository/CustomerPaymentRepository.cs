using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;

namespace DataAccess.Repository
{
    public class CustomerPaymentRepository : GenericRepository<CustomerPayment>, ICustomerPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerPaymentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(CustomerPayment customerPayment)
        {
            var customerPaymentInDb = _context.CustomerPayments.FirstOrDefault(x => x.Id == customerPayment.Id);
            if (customerPaymentInDb != null)
            {
                customerPaymentInDb.OrderHeaderId = customerPayment.OrderHeaderId;
                customerPaymentInDb.MoneySafeId = customerPayment.MoneySafeId;
                customerPaymentInDb.Amount = customerPayment.Amount;
                customerPaymentInDb.Date = customerPayment.Date;
            }
        }
    }
}
