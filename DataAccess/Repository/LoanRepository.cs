using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class LoanRepository : GenericRepository<Loan>, ILoanRepository
    {
        private readonly ApplicationDbContext _context;
        public LoanRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Loan>> GetAllLoans(DateTime? fromDate, DateTime? toDate, string selectedEmpId)
        {
            var list = new List<Loan>();
            list = await _context.Loan
                .Where(z => z.Date.Value.Date >= fromDate.Value.Date
                            && z.Date.Value.Date <= toDate.Value.Date
                            && (selectedEmpId == "0" || z.EmpId == selectedEmpId)
                            )
                .Include(z => z.Emp)
                .Include(z => z.ApplicationUser)
                .Include(z => z.MoneySafe)
                .ToListAsync();
            return list;
        }
        public void Update(Loan loan)
        {
            var loanInDb = _context.Loan.FirstOrDefault(x => x.Id == loan.Id);
            if (loanInDb != null)
            {
                loanInDb.Amount = loan.Amount;
                loanInDb.Date = loan.Date;
                loanInDb.Notes = loan.Notes;
                loanInDb.MoneySafeId = loan.MoneySafeId;
                loanInDb.EmpId = loan.EmpId;
            }
        }
    }
}
