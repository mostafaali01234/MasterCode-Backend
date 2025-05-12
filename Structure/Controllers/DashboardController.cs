using DataAccess.Repository;
using Entities.DTO;
using Entities.IRepository;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        private readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, previousMonth, 1);
        private readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DashboardController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("GetTotalOrdersRadialChartData")]
        [Authorize]
        public async Task<IActionResult> GetTotalOrdersRadialChartData()
        {
            var totalOrders = _unitOfWork.OrderHeader
                .GetAll(z => z.OrderStatus != SD.StatusNew
                            && z.OrderStatus != SD.StatusCanceled
                            && z.OrderStatus != SD.StatusDeleted);

            var ordersToReturn = _mapper.Map<IEnumerable<OrderHeaderDisplayDto>>(totalOrders);

            var countByCurrentMonth = ordersToReturn.Count(z => z.InstallDate >= currentMonthStartDate && z.InstallDate <= DateTime.Now);
            var countByPreviousMonth = ordersToReturn.Count(z => z.InstallDate >= previousMonthStartDate && z.InstallDate <= currentMonthStartDate);


            return Ok(GetRaidalChartDataModel(Convert.ToInt32(countByCurrentMonth), countByCurrentMonth, countByPreviousMonth));
        }


        [HttpGet("GetTotalRevenueRadialChartData")]
        [Authorize]
        public async Task<IActionResult> GetTotalRevenueRadialChartData()
        {
            var totalOrders = _unitOfWork.OrderHeader
                .GetAll(z => z.OrderStatus != SD.StatusNew
                            && z.OrderStatus != SD.StatusCanceled
                            && z.OrderStatus != SD.StatusDeleted);
            var ordersToReturn = _mapper.Map<IEnumerable<OrderHeaderDisplayDto>>(totalOrders);

            var totalRevenue = Convert.ToInt32(ordersToReturn.Sum(z => z.OrderTotal));

            var countByCurrentMonth = ordersToReturn
                .Where(z => z.InstallDate >= currentMonthStartDate && z.InstallDate <= DateTime.Now).Sum(z => z.OrderTotal);
            var countByPreviousMonth = ordersToReturn
                .Where(z => z.InstallDate >= previousMonthStartDate && z.InstallDate <= currentMonthStartDate).Sum(z => z.OrderTotal);



            return Ok(GetRaidalChartDataModel(Convert.ToInt32(countByCurrentMonth), countByCurrentMonth, countByPreviousMonth));
        }


        [HttpGet("GetRaidalChartDataModel")]
        [Authorize]
        private async Task<IActionResult> GetRaidalChartDataModel(int totalCount, double currentMonthCount, double previousMonthCount)
        {
            RadialBarChartDto rbc = new RadialBarChartDto();

            int increaseDecreaseRatio = 100;
            if (previousMonthCount != 0)
            {
                increaseDecreaseRatio = Convert.ToInt32((currentMonthCount - previousMonthCount) / previousMonthCount * 100);
            }

            rbc.TotalCount = (decimal)totalCount;
            rbc.CountCurrentMonth = (decimal)(currentMonthCount - previousMonthCount);
            //rbc.CountCurrentMonth = Convert.ToInt32(currentMonthCount);
            rbc.HasRatioIncreased = currentMonthCount > previousMonthCount;
            rbc.Series = new int[] { increaseDecreaseRatio };

            return Ok(rbc);
        }


        [HttpGet("GetSalesLineChartData")]
        [Authorize]
        public async Task<IActionResult> GetSalesLineChartData()
        {
            var data = _unitOfWork.OrderHeader
                .GetAll(z => z.OrderDate >= DateTime.Now.AddDays(-30) && z.OrderDate <= DateTime.Now, IncludeWord: "ApplicationUser");

            var dataToReturn = _mapper.Map<IEnumerable<OrderHeaderDisplayDto>>(data)
                .GroupBy(z => new { date = z.OrderDate.Date, name = z.ApplicationUserName })
                .Select(z => new {
                    DateTime = z.Key.date,
                    Seller = z.Key.name,
                    Count = z.Count()
                });

            var categories = dataToReturn.OrderBy(z => z.DateTime).Select(z => z.DateTime.ToString("MM/dd/yyyy")).ToArray();

            List<ChartData> cdList = new() { };

            foreach (var emp in await _userManager.Users.Include(z => z.Job).ToListAsync())
            {
                if (dataToReturn.Any(z => z.Seller == emp.Name))
                {
                    var list = new int[categories.Length];
                    var cc = 0;
                    foreach (var d in categories)
                    {
                        var item = dataToReturn.FirstOrDefault(z => z.DateTime.ToString("MM/dd/yyyy") == d && z.Seller == emp.Name);
                        list[cc] = (item != null
                            ? item.Count
                            : 0);
                        cc += 1;
                    }
                    cdList.Add(new ChartData
                    {
                        Name = emp.Name,
                        Data = list
                    });
                }
            }

            LineChartVM lc = new()
            {
                Categories = categories,
                Series = cdList
            };

            return Ok(lc);
        }

        
        [HttpGet("GetPercentSettings")]
        public async Task<IActionResult> GetPercentSettings()
        {
            var list = await _unitOfWork.Setting.GetAll(z => z.Name.Contains("Percent"));

            return Ok(list);
        }


        [HttpPost("AddUpdateSettings")]
        [Authorize]
        public async Task<IActionResult> AddUpdateSetting(Settings settings)
        {
            try
            {
                var old = await _unitOfWork.Setting.GetFirstorDefault(z => z.Name == settings.Name);
                if(old == null)
                {
                    settings.Id = 0;
                    _unitOfWork.Setting.Add(settings);
                }
                else
                {
                    _unitOfWork.Setting.Update(settings);
                }
                await _unitOfWork.Complete();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
