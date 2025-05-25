using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;
using Entities.DTO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        private readonly ApplicationDbContext _context;
        public ExpenseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Expense>> GetAllExpenses(DateTime? fromDate, DateTime? toDate, string selectedEmpId, int selectedExpenseTypeId)
        {
            var list = new List<Expense>();
            list = await _context.Expenses
                .Where(z => z.Date.Value.Date >= fromDate.Value.Date 
                            && z.Date.Value.Date <= toDate.Value.Date
                            && (selectedEmpId == "0" || z.EmpId == selectedEmpId)
                            && (selectedExpenseTypeId == 0 || z.ExpenseTypeId == selectedExpenseTypeId)
                            )
                .Include(z => z.ExpenseType)
                .Include(z => z.Emp)
                .Include(z => z.ApplicationUser)
                .Include(z => z.MoneySafe)
                .ToListAsync();
            return list;
        }

        public void Update(Expense expense)
        {
            var expenseInDb = _context.Expenses.FirstOrDefault(x => x.Id == expense.Id);
            if (expenseInDb != null)
            {
                expenseInDb.ExpenseTypeId = expense.ExpenseTypeId;
                expenseInDb.Amount = expense.Amount;
                expenseInDb.Date = expense.Date;
                expenseInDb.Notes = expense.Notes;
                expenseInDb.EmpId = expense.EmpId;
                expenseInDb.MoneySafeId = expense.MoneySafeId;
            }
        }
    }
}
