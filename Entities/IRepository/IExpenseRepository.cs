using Entities.Models;

namespace Entities.IRepository
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        void Update(Expense expense);
    }
}
