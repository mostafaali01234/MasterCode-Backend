using Entities.Models;

namespace Entities.IRepository
{
    public interface IExpenseTypeRepository : IGenericRepository<ExpenseType>
    {
        void Update(ExpenseType expenseType);
    }
}
