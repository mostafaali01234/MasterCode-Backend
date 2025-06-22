using Entities.DTO;
using Entities.Models;

namespace Entities.IRepository
{
    public interface ISettingRepository : IGenericRepository<Settings>
    {
        void Update(Settings settings);
        IEnumerable<CommissionDto> GetOrderComms(int orderId);
        IEnumerable<UserBalacneDto> GetEmpComms(string empId, DateTime fromDate, DateTime toDate);
    }
}
