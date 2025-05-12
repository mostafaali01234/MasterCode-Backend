using Entities.Models;

namespace Entities.IRepository
{
    public interface IMoneySafeRepository : IGenericRepository<MoneySafe>
    {
        void Update(MoneySafe moneySafe);
    }
}
