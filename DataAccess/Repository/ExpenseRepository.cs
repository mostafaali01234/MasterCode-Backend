using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;

namespace DataAccess.Repository
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        private readonly ApplicationDbContext _context;
        public ExpenseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
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
