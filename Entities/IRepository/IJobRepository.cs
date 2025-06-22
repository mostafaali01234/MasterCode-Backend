using Entities.Models;

namespace Entities.IRepository
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        void Update(Job job);
    }
}
