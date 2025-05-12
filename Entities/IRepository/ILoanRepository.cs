using Entities.Models;

namespace Entities.IRepository
{
    public interface ILoanRepository : IGenericRepository<Loan>
    {
        void Update(Loan loan);
    }
}
