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
    public class CustomerPaymentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CustomerPaymentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCustomerPayment(int id)
        {
            try
            {
                var customerPayment = await _unitOfWork.CustomerPayment.GetFirstorDefault(c => c.Id == id, IncludeWord: "OrderHeader,MoneySafe,ApplicationUser");
                if (customerPayment == null)
                {
                    return NotFound("CustomerPayment not found");
                }
                var customerPaymentToReturn = _mapper.Map<CustomerPaymentDisplayDto>(customerPayment);
                return Ok(customerPaymentToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customerPayments = await _unitOfWork.CustomerPayment.GetAll(IncludeWord: "OrderHeader,MoneySafe,ApplicationUser");
                var customerPaymentsToReturn = _mapper.Map<IEnumerable<CustomerPaymentDisplayDto>>(customerPayments);
                return Ok(customerPaymentsToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = SD.AdminRole)]
        public async Task<IActionResult> DeleteCustomerPayment(int id)
        {
            try
            {
                var customerPayment = await _unitOfWork.CustomerPayment.GetFirstorDefault(c => c.Id == id);
                if (customerPayment == null)
                {
                    return NotFound("CustomerPayment not found");
                }
                _unitOfWork.CustomerPayment.Remove(customerPayment);
                await _unitOfWork.Complete();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerPayment(int id, [FromBody] CustomerPaymentUpdateDto customerPaymentToUpdate)
        {
            try
            {
                if (id != customerPaymentToUpdate.Id)
                {
                    return BadRequest("CustomerPayment ID mismatch");
                }
                var existingCustomerPayment = await _unitOfWork.CustomerPayment.GetFirstorDefault(c => c.Id == id);
                if (existingCustomerPayment == null)
                {
                    return NotFound("CustomerPayment not found");
                }
                var customerPayment = _mapper.Map<CustomerPayment>(customerPaymentToUpdate);
                _unitOfWork.CustomerPayment.Update(customerPayment);
                await _unitOfWork.Complete();
                return Ok(customerPaymentToUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCustomerPayment(CustomerPaymentCreateDto customerPaymentToAdd)
        {
            try
            {
                var customerPayment = _mapper.Map<CustomerPayment>(customerPaymentToAdd);
                customerPayment.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _unitOfWork.CustomerPayment.Add(customerPayment);
                await _unitOfWork.Complete();
                return CreatedAtAction(nameof(AddCustomerPayment), customerPayment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
