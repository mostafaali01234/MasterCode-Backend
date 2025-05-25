using Entities.Models;

namespace Entities.IRepository
{
    public interface ILoanRepository : IGenericRepository<Loan>
    {
        void Update(Loan loan);
        Task<List<Loan>> GetAllLoans(DateTime? fromDate, DateTime? toDate, string selectedEmpId);
    }
}
