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
using System.Text.Json;

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
        public async Task<IActionResult> GetOrders(DateTime fromCreateDate
                                                    , DateTime toCreateDate, string selectedSellerId
                                                    , string selectedTechId, string? selectedCustomerName
                                                    , string selectedOrderStatus, string selectedPaymentStatus
                                                    , int? OrderNo = 0)
        {
            if(fromCreateDate == null)
                fromCreateDate = DateTime.Now;
             if(toCreateDate == null)
                toCreateDate = DateTime.Now;

             if(selectedOrderStatus == null)
                selectedOrderStatus = "الكل";
             if(selectedPaymentStatus == null)
                selectedPaymentStatus = "الكل";

            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole(SD.AdminRole);

            var orders = await _unitOfWork.OrderHeader
                .GetAll(x => 
                    (isAdmin ? (selectedSellerId == "0" || x.ApplicationUserId == selectedSellerId) : x.ApplicationUserId == user) 
                    && (x.OrderDate >= fromCreateDate
                    && x.OrderDate < toCreateDate.AddDays(1)
                    && (selectedTechId == "0" || x.TechId == selectedTechId)
                    //&& (isAdmin ? (selectedTechId == "0" || x.TechId == selectedTechId) : x.TechId == selectedTechId) 
                    && (selectedOrderStatus == "الكل" || x.OrderStatus == selectedOrderStatus)
                    && (selectedPaymentStatus == "الكل" || x.PaymentStatus == selectedPaymentStatus)
                    && (selectedCustomerName == "" || selectedCustomerName == null || x.Customer.CustomerName.Contains(selectedCustomerName) || x.Customer.CustomerPhoneNumber.Contains(selectedCustomerName) )
                    )
                    , IncludeWord: "ApplicationUser,Customer,Tech");

            if (OrderNo != 0)
            {
                orders = await _unitOfWork.OrderHeader.GetAll(z => z.Id == OrderNo, IncludeWord: "ApplicationUser,Customer,Tech");
            }
            var ordersToReturn = _mapper.Map<IEnumerable<OrderHeaderDisplayDto>>(orders);

            return Ok(ordersToReturn);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var orderHeader = await _unitOfWork.OrderHeader.GetFirstorDefault(x => x.Id == id , IncludeWord: "ApplicationUser,Customer,Tech");
            var orderPayment = await _unitOfWork.CustomerPayment.GetFirstorDefault(x => x.OrderHeaderId == id , IncludeWord: "ApplicationUser,MoneySafe");
            var orderDets = await _unitOfWork.OrderDetail.GetAll(z => z.OrderHeaderId == id, IncludeWord: "Product");
            

            var orderToReturn = new OrderDisplayDto
            {
                OrderHeader = _mapper.Map<OrderHeaderDisplayDto>(orderHeader),
                OrderDetailsList = _mapper.Map<IEnumerable<OrderDetailDisplayDto>>(orderDets),
                OrderPayment = _mapper.Map<CustomerPaymentDisplayDto>(orderPayment),
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
        public async Task<IActionResult> CancelOrder([FromBody]int id)
        {
            var old = _unitOfWork.OrderHeader.GetFirstorDefault(z => z.Id == id);
            if (old == null)
                return BadRequest("Order Not Found!!");


            //var orderToUpdate = _mapper.Map<OrderHeader>(old);
            _unitOfWork.OrderHeader.CancelOrder(old.Result);
            await _unitOfWork.Complete();


            return Ok(old.Result);
        }

        [HttpPost]
        [Authorize]
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
                //return RedirectToAction("GetOrderDetails", orderHeader.Id);


                var orderToReturn = new OrderDisplayDto
                {
                    OrderHeader = _mapper.Map<OrderHeaderDisplayDto>(orderHeader),
                    OrderDetailsList = _mapper.Map<IEnumerable<OrderDetailDisplayDto>>(orderDets)
                };
                return Ok(orderToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateDto orderToUpdate)
        {
            try
            {
                if (id != orderToUpdate.OrderHeader.Id)
                {
                    return BadRequest("Order ID mismatch");
                }
                var existingOrder = await _unitOfWork.OrderHeader.GetFirstorDefault(c => c.Id == id);
                if (existingOrder == null)
                {
                    return NotFound("Order not found");
                }
                var orderHeader = _mapper.Map<OrderHeader>(orderToUpdate.OrderHeader);
                var orderDets = _mapper.Map<OrderDetail[]>(orderToUpdate.OrderDetailsList);
                orderHeader.OrderTotal = orderDets.Sum(z => z.Count * z.Price);
                _unitOfWork.OrderHeader.Update(orderHeader);
                var oldDets = await _unitOfWork.OrderDetail.GetAll(z => z.OrderHeaderId == id);
                _unitOfWork.OrderDetail.RemoveRange(oldDets);
                foreach(var item in orderDets)
                {
                    item.Id = 0;
                    item.OrderHeaderId = orderHeader.Id;
                    //if (item.Id != 0)
                    //    _unitOfWork.OrderDetail.Update(item);
                    //else
                        _unitOfWork.OrderDetail.Add(item);
                }
                await _unitOfWork.Complete();
                return Ok(orderToUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("CompleteOrder")]
        //public async Task<IActionResult> CompleteOrder(int id, double Paid, int MoneySafeId, DateTime CompleteDate)
        public async Task<IActionResult> CompleteOrder(OrderCompleteDto completeDets)
        {
            var old = _unitOfWork.OrderHeader.GetFirstorDefault(z => z.Id == completeDets.orderId);
            if (old.Result == null)
                return BadRequest("Order Not Found!!");

            old.Result.InstallDate = completeDets.CompleteDate;

            var orderToUpdate = _mapper.Map<OrderHeader>(old.Result);
            if (orderToUpdate.OrderStatus == SD.StatusDone)
            {
                var paymentToRemove = await _unitOfWork.CustomerPayment
                    .GetAll(z => z.OrderHeaderId == orderToUpdate.Id/* && z.Date == completeDets.CompleteDate*/);
                if (paymentToRemove != null)
                {
                    _unitOfWork.CustomerPayment.RemoveRange(paymentToRemove);
                    await _unitOfWork.Complete();
                }
            }
            _unitOfWork.CustomerPayment.Add(new CustomerPayment
            {
                Amount = completeDets.Paid,
                OrderHeaderId = orderToUpdate.Id,
                Date = completeDets.CompleteDate,
                MoneySafeId = completeDets.MoneySafeId,
                ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            });
            if (orderToUpdate.OrderTotal == completeDets.Paid)
                orderToUpdate.PaymentStatus = SD.PaymentStatusDonw;
            _unitOfWork.OrderHeader.CompleteOrder(orderToUpdate, completeDets.TechId);
            await _unitOfWork.Complete();

            return Ok(orderToUpdate);
        }
        
      

        [HttpGet("GetOrderComms")]
        //[Authorize]
        public async Task<IActionResult> GetOrderComms(int orderId)
        {
            var old = _unitOfWork.OrderHeader.GetFirstorDefault(z => z.Id == orderId);
            if (old.Result == null)
                return NotFound(new { message = "الاوردر غير موجود", status = 400 });

            var list = _unitOfWork.Setting.GetOrderComms(orderId);
            if(list.Count() > 0)
                return Ok(list);
            else
                return NotFound(new { message = "الاوردر غير تام-غير مدفوع", status = 400 });
        }

        
    }
}
