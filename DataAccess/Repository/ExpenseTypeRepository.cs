using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;

namespace DataAccess.Repository
{
    public class ExpenseTypeRepository : GenericRepository<ExpenseType>, IExpenseTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public ExpenseTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ExpenseType expenseType)
        {
            var expenseTypeInDb = _context.ExpenseTypes.FirstOrDefault(x => x.Id == expenseType.Id);
            if (expenseTypeInDb != null)
            {
                expenseTypeInDb.Name = expenseType.Name;
                expenseTypeInDb.Description = expenseType.Description;
            }
        }
    }
}
