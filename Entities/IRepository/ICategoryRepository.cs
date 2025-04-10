using Entities.Models;

namespace Entities.IRepository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        void Update(Category category);
    }
}
