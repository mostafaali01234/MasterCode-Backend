using Entities.Models;

namespace Entities.IRepository
{
    public interface ISettingRepository : IGenericRepository<Settings>
    {
        void Update(Settings settings);
    }
}
