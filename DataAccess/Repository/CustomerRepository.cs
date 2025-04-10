using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;

namespace DataAccess.Repository
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Customer customer)
        {
            var CustomerInDb = _context.Customers.FirstOrDefault(x => x.Id == customer.Id);
            if (CustomerInDb != null)
            {
                CustomerInDb.CustomerName = customer.CustomerName;
                CustomerInDb.CustomerPhoneNumber = customer.CustomerPhoneNumber;
                CustomerInDb.CustomerAddress = customer.CustomerAddress;
            }
        }
    }
}
