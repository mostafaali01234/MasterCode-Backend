using Entities.DTO;
using Entities.Models;

namespace Entities.IRepository
{
    public interface IMoneySafeRepository : IGenericRepository<MoneySafe>
    {
        void Update(MoneySafe moneySafe);
        double GetCurrentBalance(int id);
        List<MoneySafeMovesDisplayDto> GetMoneysafeMoves(int id, DateTime? fromDate, DateTime? endDate);
    }
}
