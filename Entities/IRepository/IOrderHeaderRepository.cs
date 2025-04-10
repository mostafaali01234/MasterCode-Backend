using Entities.Models;

namespace Entities.IRepository
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
    }
}
