using Entities.DTO;
using Entities.Models;

namespace Entities.IRepository
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        void Update(Expense expense);
        Task<List<Expense>> GetAllExpenses(DateTime? fromDate, DateTime? toDate, string selectedEmpId, int selectedExpenseTypeId);
    }
}
