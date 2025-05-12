using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;

namespace DataAccess.Repository
{
    public class MoneySafeRepository : GenericRepository<MoneySafe>, IMoneySafeRepository
    {
        private readonly ApplicationDbContext _context;
        public MoneySafeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(MoneySafe moneySafe)
        {
            var moneySafeInDb = _context.MoneySafes.FirstOrDefault(x => x.Id == moneySafe.Id);
            if (moneySafeInDb != null)
            {
                moneySafeInDb.Name = moneySafe.Name;
                moneySafeInDb.OpeningBalance = moneySafe.OpeningBalance;
                moneySafeInDb.ApplicationUserId = moneySafe.ApplicationUserId;
            }
        }
    }
}
