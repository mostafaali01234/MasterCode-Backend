using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;

namespace DataAccess.Repository
{
    public class LoanRepository : GenericRepository<Loan>, ILoanRepository
    {
        private readonly ApplicationDbContext _context;
        public LoanRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
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
