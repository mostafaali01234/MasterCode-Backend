using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;
using Entities.DTO;

namespace DataAccess.Repository
{
    public class SettingRepository : GenericRepository<Settings>, ISettingRepository
    {
        private readonly ApplicationDbContext _context;
        public SettingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<UserBalacneDto> GetEmpComms(string empId, DateTime fromDate, DateTime toDate)
        {
            var techCommPercent = double.Parse(_context.Settings.FirstOrDefault(z => z.Name == "TechPercent").Value ?? "0");
            var sellerCommPercent = double.Parse(_context.Settings.FirstOrDefault(z => z.Name == "SellerPercent").Value ?? "0");
            var marketingCommPercent = double.Parse(_context.Settings.FirstOrDefault(z => z.Name == "MareketingPercent").Value ?? "0");
            var adsCommPercent = double.Parse(_context.Settings.FirstOrDefault(z => z.Name == "AdsPercent").Value ?? "0");
            var programmerCommPercent = double.Parse(_context.Settings.FirstOrDefault(z => z.Name == "ProgramerPercent").Value ?? "0");
            var emp = _context.ApplicationUsers.FirstOrDefault(x => x.Id == empId.ToString());
            var list = new List<UserBalacneDto>();
            var retlist = new List<UserBalacneDto>();

            #region عمولات
            var TechSellerOrders = _context.OrderHeaders
                .Where(x =>
                        x.OrderStatus == "اوردر تام"
                        && x.PaymentStatus == "تم الدفع"
                        && x.PaymentStatus == "تم الدفع"
                        //&& x.InstallDate >= fromDate
                        //&& x.InstallDate < toDate.AddDays(1)
                        && (x.TechId == empId || x.ApplicationUserId == empId) 
                      ).Select(x => new UserBalacneDto
                      {
                          OperationId = x.Id,
                          OperationType = "عمولة -" + (x.TechId == empId ? "فني" : "بائع") + "- اوردر رقم:" + x.Id,
                          OperationDate = x.InstallDate,
                          MoneysafeName = "",
                          OperationNotes = "",
                          Balance = 0,
                          OperationTotal = x.OrderTotal * ((x.TechId == empId ? techCommPercent : sellerCommPercent) / 100.0),
                      })
                .ToList();

            if (emp.JobId == 4)
            {
                list.AddRange(_context.OrderHeaders
                .Where(x =>
                        x.OrderStatus == "اوردر تام"
                        && x.PaymentStatus == "تم الدفع"
                        && x.PaymentStatus == "تم الدفع"
                        //&& x.InstallDate >= fromDate
                        //&& x.InstallDate < toDate.AddDays(1)
                      ).Select(x => new UserBalacneDto
                      {
                          //OrderId = x.Id,
                          //OrderTotal = x.OrderTotal,
                          //InstallDate = x.InstallDate,
                          //EmpId = empId,
                          //EmpName = "تسويق",
                          //Comm = x.OrderTotal * (marketingCommPercent / 100.0)

                          OperationId = x.Id,
                          OperationType = "عمولة -تسويق- اوردر رقم:" + x.Id,
                          OperationDate = x.InstallDate,
                          MoneysafeName = "",
                          OperationNotes = "",
                          Balance = 0,
                          OperationTotal = x.OrderTotal * (marketingCommPercent / 100.0),
                      })
                .ToList());
            }
             if (emp.JobId == 2)
            {
                list.AddRange(_context.OrderHeaders
                .Where(x =>
                        x.OrderStatus == "اوردر تام"
                        && x.PaymentStatus == "تم الدفع"
                        && x.PaymentStatus == "تم الدفع"
                        //&& x.InstallDate >= fromDate
                        //&& x.InstallDate < toDate.AddDays(1)
                      ).Select(x => new UserBalacneDto
                      {
                          //OrderId = x.Id,
                          //OrderTotal = x.OrderTotal,
                          //InstallDate = x.InstallDate,
                          //EmpId = empId,
                          //EmpName = "برمجه",
                          //Comm = x.OrderTotal * (programmerCommPercent / 100.0)

                          OperationId = x.Id,
                          OperationType = "عمولة -برمجه- اوردر رقم:" + x.Id,
                          OperationDate = x.InstallDate,
                          MoneysafeName = "",
                          OperationNotes = "",
                          Balance = 0,
                          OperationTotal = x.OrderTotal * (programmerCommPercent / 100.0),
                      })
                .ToList());
            }
            list.AddRange(TechSellerOrders);
            #endregion

            #region سلف
            var loans = _context.Loan
                .Where(x =>
                        x.EmpId == empId
                        //&& x.Date >= fromDate
                        //&& x.Date < toDate.AddDays(1)
                      ).Select(x => new UserBalacneDto
                      {
                          OperationId = x.Id,
                          OperationType = "سلف",
                          OperationDate = (DateTime)x.Date,
                          MoneysafeName = x.MoneySafe.Name,
                          OperationNotes = x.Notes,
                          OperationTotal = (double)x.Amount,
                          Balance = 0,
                      })
                .ToList();

            list.AddRange(loans);
            #endregion

            #region balanceCalc
            double runningBalance = 0;
            list = list.OrderBy(z => z.OperationDate).ToList();
            foreach (var t in list.OrderBy(t => t.OperationDate))
            {
                runningBalance += (t.OperationType == "سلف" ? -1 * t.OperationTotal : (t.OperationType.Contains("عمولة") ? t.OperationTotal : 0));
                t.Balance += runningBalance;
            }
            #endregion

            #region رصيد-سابق
            var oldBalance = list.Where(x => x.OperationDate < fromDate).OrderByDescending(z => z.OperationDate).FirstOrDefault();
            retlist = list.Where(x => x.OperationDate >= fromDate && x.OperationDate < toDate.AddDays(1)).ToList();
            retlist.Insert(0, new UserBalacneDto
            {
                OperationType = "رصيد سابق",
                OperationTotal = (oldBalance == null ? 0 : oldBalance.Balance),
                Balance = (oldBalance == null ? 0 : oldBalance.Balance)
            });
            #endregion

            return retlist;
        }

        public IEnumerable<CommissionDto> GetOrderComms(int orderId)
        {
            var techCommPercent = double.Parse(_context.Settings.FirstOrDefault(z => z.Name == "TechPercent").Value ?? "0");
            var sellerCommPercent = double.Parse(_context.Settings.FirstOrDefault(z => z.Name == "SellerPercent").Value ?? "0");
            var marketingCommPercent = double.Parse(_context.Settings.FirstOrDefault(z => z.Name == "MareketingPercent").Value ?? "0");
            var adsCommPercent = double.Parse(_context.Settings.FirstOrDefault(z => z.Name == "AdsPercent").Value ?? "0");
            var programmerCommPercent = double.Parse(_context.Settings.FirstOrDefault(z => z.Name == "ProgramerPercent").Value ?? "0");

            var list = new List<CommissionDto>();


            var tech = _context.OrderHeaders
               .Where(x => x.Id == orderId && x.OrderStatus == "اوردر تام" && x.PaymentStatus == "تم الدفع")
               .Select(x => new CommissionDto
               {
                   OrderId = x.Id,
                   OrderTotal = x.OrderTotal,
                   InstallDate = x.InstallDate,
                   EmpId = x.TechId,
                   EmpName = x.Tech.Name,
                   Comm = x.OrderTotal * (techCommPercent / 100.0)
               }).FirstOrDefault();
            var seller = _context.OrderHeaders
               .Where(x => x.Id == orderId && x.OrderStatus == "اوردر تام" && x.PaymentStatus == "تم الدفع")
               .Select(x => new CommissionDto
               {
                   OrderId = x.Id,
                   OrderTotal = x.OrderTotal,
                   InstallDate = x.InstallDate,
                   EmpId = x.ApplicationUserId,
                   EmpName = x.ApplicationUser.Name,
                   Comm = x.OrderTotal * (sellerCommPercent / 100.0)
               }).FirstOrDefault();
            var ads = _context.OrderHeaders
               .Where(x => x.Id == orderId && x.OrderStatus == "اوردر تام" && x.PaymentStatus == "تم الدفع")
               .Select(x => new CommissionDto
               {
                   OrderId = x.Id,
                   OrderTotal = x.OrderTotal,
                   InstallDate = x.InstallDate,
                   EmpId = "",
                   EmpName = "اعلانات",
                   Comm = x.OrderTotal * (adsCommPercent / 100.0)
               }).FirstOrDefault(); 
            var marketing = _context.OrderHeaders
               .Where(x => x.Id == orderId && x.OrderStatus == "اوردر تام" && x.PaymentStatus == "تم الدفع")
               .Select(x => new CommissionDto
               {
                   OrderId = x.Id,
                   OrderTotal = x.OrderTotal,
                   InstallDate = x.InstallDate,
                   EmpId = "",
                   EmpName = "تسويق",
                   Comm = x.OrderTotal * (marketingCommPercent / 100.0)
               }).FirstOrDefault();
            var programmer = _context.OrderHeaders
               .Where(x => x.Id == orderId && x.OrderStatus == "اوردر تام" && x.PaymentStatus == "تم الدفع")
               .Select(x => new CommissionDto
               {
                   OrderId = x.Id,
                   OrderTotal = x.OrderTotal,
                   InstallDate = x.InstallDate,
                   EmpId = "",
                   EmpName = "برمجه",
                   Comm = x.OrderTotal * (programmerCommPercent / 100.0)
               }).FirstOrDefault();

            if(tech != null)
                list.Add(tech);
            if (seller != null)
                list.Add(seller);
            if (ads != null)
                list.Add(ads);
            if (programmer != null)
                list.Add(programmer);
            if (marketing != null)
                list.Add(marketing);

            return list.OrderBy(z => z.InstallDate).ThenBy(z => z.OrderId).ToList();
        }

        public void Update(Settings settings)
        {
            var settingsInDb = _context.Settings.FirstOrDefault(x => x.Name == settings.Name);
            if (settingsInDb != null)
            {
                settingsInDb.Value = settings.Value;
            }
        }
    }
}
