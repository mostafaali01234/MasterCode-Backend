using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;
using Entities.DTO;
using System.Collections.Generic;
using Utilities;

namespace DataAccess.Repository
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public List<CustomerMovesDisplayDto> GetCustomerMoves(int id, DateTime? fromDate, DateTime? endDate)
        {
            var moves = new List<CustomerMovesDisplayDto>();

            var customerOrders = (from c in _context.OrderHeaders
                                where c.CustomerId == id && c.OrderStatus == SD.StatusDone
                                select new CustomerMovesDisplayDto
                                {
                                    opId = c.Id,
                                    opDate = c.InstallDate,
                                    opType = "اوردر",
                                    opNotes = "اوردر رقم: " + c.Id.ToString(),
                                    opTotal = c.OrderTotal,
                                    balance = 0
                                }).ToList();

            var customerPays = (from c in _context.CustomerPayments
                                where c.OrderHeader.CustomerId == id
                                select new CustomerMovesDisplayDto
                                {
                                    opId = c.Id,
                                    opDate = c.Date,
                                    opType = "دفعات عملاء",
                                    opNotes = "دفعة اوردر رقم: " + c.OrderHeader.Id.ToString(),
                                    opTotal = c.Amount,
                                    balance = 0
                                }).ToList();


            moves.AddRange(customerPays);
            moves.AddRange(customerOrders);
            moves = moves.OrderBy(z => z.opDate).ThenBy(z => z.opType).ToList();

            double sum = 0;
            foreach (var m in moves)
            {
                sum += (m.opType == "اوردر" ? m.opTotal ?? 0 : (m.opTotal ?? 0) * -1);
                m.balance = sum;
            }
            var prevItem = moves.Where(z => z.opDate < fromDate)/*.OrderByDescending(z => z.opDate)*/.LastOrDefault();
            var retList = new List<CustomerMovesDisplayDto>();
            retList.Add(new CustomerMovesDisplayDto
            {
                opId = 0,
                opDate = null,
                opType = "رصيد سابق",
                opNotes = "",
                opTotal = prevItem == null ? 0 : prevItem.balance,
                balance = prevItem == null ? 0 : prevItem.balance
            });

            retList.AddRange(moves.Where(z => z.opDate >= fromDate).ToList());

            return retList;
        }

        public void Update(Customer customer)
        {
            var CustomerInDb = _context.Customers.FirstOrDefault(x => x.Id == customer.Id);
            if (CustomerInDb != null)
            {
                CustomerInDb.CustomerName = customer.CustomerName;
                CustomerInDb.CustomerPhoneNumber = customer.CustomerPhoneNumber;
                CustomerInDb.CustomerAddress = customer.CustomerAddress;
            }
        }
    }
}
