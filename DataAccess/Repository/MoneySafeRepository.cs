using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;
using Entities.DTO;
using System.Collections.Generic;

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

        public double GetCurrentBalance(int id)
        {
            var moneySafe = _context.MoneySafes.FirstOrDefault(x => x.Id == id);
            if (moneySafe != null)
            {
                var openingBalance = moneySafe.OpeningBalance ?? 0;
                var totalIncome = _context.CustomerPayments.Where(x => x.MoneySafeId == id).Sum(x => x.Amount) ?? 0;
                var totalExpense = _context.Expenses.Where(x => x.MoneySafeId == id).Sum(x => x.Amount) ?? 0;
                var totalLoan = _context.Loan.Where(x => x.MoneySafeId == id).Sum(x => x.Amount) ?? 0;

                var balance = openingBalance + totalIncome - totalExpense - totalLoan;

                return balance;
            }
            else
            {
                throw new Exception("MoneySafe not found");
            }
        }

        
        public List<MoneySafeMovesDisplayDto> GetMoneysafeMoves(int id, DateTime? fromDate, DateTime? endDate)
        {
            var moves = new List<MoneySafeMovesDisplayDto>();

            var moneySafeOpeningBalance = new MoneySafeMovesDisplayDto
            {
                opId = 0,
                opDate = DateTime.Parse("2024-01-01"),
                opType = "رصيد افتتاحي",
                opNotes = "",
                opTotal = _context.MoneySafes.Find(id).OpeningBalance ?? 0,
                balance = _context.MoneySafes.Find(id).OpeningBalance ?? 0
            };

            var customerPays = (from c in _context.CustomerPayments
                                  where c.MoneySafeId == id /*&& (fromDate == null || c.Date >= fromDate) && (endDate == null || c.Date <= endDate)*/
                                  select new MoneySafeMovesDisplayDto
                                  {
                                      opId = c.Id,
                                      opDate = c.Date,
                                      opType = "دفعات عملاء",
                                      opNotes = "دفعة اوردر رقم: " + c.OrderHeader.Id.ToString(),
                                      opTotal = c.Amount,
                                      balance = 0
                                  }).ToList();

            var expenseMoves = (from e in _context.Expenses
                                where e.MoneySafeId == id /*&& (fromDate == null || e.Date >= fromDate) && (endDate == null || e.Date <= endDate)*/
                                select new MoneySafeMovesDisplayDto
                                {
                                    opId = e.Id,
                                    opDate = e.Date,
                                    opType = "مصروفات",
                                    opNotes = e.Notes,
                                    opTotal = e.Amount,
                                    balance = 0
                                }).ToList();

            var loanMoves = (from l in _context.Loan
                             where l.MoneySafeId == id /*&& (fromDate == null || l.Date >= fromDate) && (endDate == null || l.Date <= endDate)*/
                             select new MoneySafeMovesDisplayDto
                             {
                                 opId = l.Id,
                                 opDate = l.Date,
                                 opType = "سلف",
                                 opNotes = l.Notes,
                                 opTotal = l.Amount,
                                 balance = 0
                             }).ToList();

            moves.Add(moneySafeOpeningBalance);
            moves.AddRange(customerPays);
            moves.AddRange(expenseMoves);
            moves.AddRange(loanMoves);
            moves = moves.OrderBy(z => z.opDate).ToList();

            double sum = 0;
            foreach(var m in moves)
            {
                sum += (m.opId == 0 || m.opType == "دفعات عملاء" ? m.opTotal ?? 0 : (m.opTotal ?? 0) * -1);
                m.balance = sum;
            }
            return moves;
        }

    }
}
