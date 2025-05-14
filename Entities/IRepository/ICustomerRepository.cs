using Entities.DTO;
using Entities.Models;

namespace Entities.IRepository
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        void Update(Customer customer);
        List<CustomerMovesDisplayDto> GetCustomerMoves(int id, DateTime? fromDate, DateTime? endDate);
    }
}
