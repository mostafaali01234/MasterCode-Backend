using Entities.Models;

namespace Entities.IRepository
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        void Update(Customer customer);
    }
}
