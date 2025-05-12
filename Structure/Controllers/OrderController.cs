using DataAccess.Repository;
using Entities.DTO;
using Entities.IRepository;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Utilities;
using System.Security.Claims;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrders(string status, DateTime fromDate, DateTime toDate)
        {
            if(fromDate == null)
                fromDate = DateTime.Now;
             if(toDate == null)
                fromDate = DateTime.Now;
             if(status == null)
                status = "الكل";

            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole(SD.AdminRole);

            var orders = await _unitOfWork.OrderHeader
                .GetAll(x => 
                    (isAdmin || x.ApplicationUserId == user) 
                    && (status == "الكل" || x.OrderStatus == status)
                , IncludeWord: "ApplicationUser,Customer,Tech");
            var ordersToReturn = _mapper.Map<IEnumerable<OrderHeaderDisplayDto>>(orders);

            return Ok(ordersToReturn);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var orderHeader = await _unitOfWork.OrderHeader.GetFirstorDefault(x => x.Id == id , IncludeWord: "ApplicationUser,Customer,Tech");
            var orderDets = await _unitOfWork.OrderDetail.GetAll(z => z.OrderHeaderId == id, IncludeWord: "Product");
            

            var orderToReturn = new OrderDisplayDto
            {
                OrderHeader = _mapper.Map<OrderHeaderDisplayDto>(orderHeader),
                OrderDetailsList = _mapper.Map<IEnumerable<OrderDetailDisplayDto>>(orderDets)
            };

            return Ok(orderToReturn);
        }
        [HttpGet("GetOrderStatus")]
        public async Task<IActionResult> GetOrderStatus()
        {
            var list = new string[] { SD.StatusNew, SD.StatusDone, SD.StatusCanceled, SD.StatusDeleted };
            return Ok(list);
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var old = _unitOfWork.OrderHeader.GetFirstorDefault(z => z.Id == id);
            if (old == null)
                return BadRequest("Order Not Found!!");


            var orderToUpdate = _mapper.Map<OrderHeader>(old);
            _unitOfWork.OrderHeader.Update(orderToUpdate);
            await _unitOfWork.Complete();


            return Ok(orderToUpdate);
        }


        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> AddOrder(OrderCreateDto orderToAdd)
        {
            try
            {
                var orderHeader = _mapper.Map<OrderHeader>(orderToAdd.OrderHeader);
                var orderDets = _mapper.Map<IEnumerable<OrderDetail>>(orderToAdd.OrderDetailsList);


                orderHeader.OrderStatus = SD.StatusNew;
                orderHeader.PaymentStatus = SD.PaymentStatusNew;
                orderHeader.OrderTotal = orderDets.Sum(z => z.Count * z.Price);
                _unitOfWork.OrderHeader.Add(orderHeader);
                await _unitOfWork.Complete();

                foreach (var dets in orderDets)
                {
                    dets.OrderHeaderId = orderHeader.Id;
                    _unitOfWork.OrderDetail.Add(dets);
                }
                await _unitOfWork.Complete();

                return RedirectToAction("GetOrderDetails", orderHeader.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
