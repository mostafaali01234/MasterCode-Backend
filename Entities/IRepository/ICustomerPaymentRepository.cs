using Entities.Models;

namespace Entities.IRepository
{
    public interface ICustomerPaymentRepository : IGenericRepository<CustomerPayment>
    {
        void Update(CustomerPayment customerPayment);
    }
}
