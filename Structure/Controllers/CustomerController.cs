using DataAccess.Repository;
using Entities.DTO;
using Entities.IRepository;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using Utilities;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CustomerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
              

                var customer = await _unitOfWork.Customer.GetFirstorDefault(c => c.Id == id, IncludeWord: "ApplicationUser");
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }
                var customerToReturn = _mapper.Map<CustomerDisplayDto>(customer);
                return Ok(customerToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpGet("getCustomerNamePhone")]
        [Authorize]
        public async Task<IActionResult> GetCustomerNamePhone(string searchText)
        {
            try
            {
                var customer = await _unitOfWork.Customer.GetFirstorDefault(c => c.CustomerName.Contains(searchText) || c.CustomerPhoneNumber.Contains(searchText), IncludeWord: "ApplicationUser");
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }
                var customerToReturn = _mapper.Map<CustomerDisplayDto>(customer);
                return Ok(customerToReturn);
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
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole(SD.AdminRole);

            try
            {
                var customers = await _unitOfWork.Customer.GetAll(x => /*(isAdmin || x.ApplicationUserId == user)*/true , IncludeWord: "ApplicationUser");
                var customersToReturn = _mapper.Map<IEnumerable<CustomerDisplayDto>>(customers);
                return Ok(customersToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = SD.AdminRole)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customer = await _unitOfWork.Customer.GetFirstorDefault(c => c.Id == id);
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }
                _unitOfWork.Customer.Remove(customer);
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
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerUpdateDto customerToUpdate)
        {
            try
            {
                if (id != customerToUpdate.Id)
                {
                    return BadRequest("Customer ID mismatch");
                }
                var existingCustomer = await _unitOfWork.Customer.GetFirstorDefault(c => c.Id == id);
                if (existingCustomer == null)
                {
                    return NotFound("Customer not found");
                }
                var customer = _mapper.Map<Customer>(customerToUpdate);
                _unitOfWork.Customer.Update(customer);
                await _unitOfWork.Complete();
                return Ok(customerToUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCustomer(CustomerCreateDto customerToAdd)
        {
            try
            {
                customerToAdd.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var customer = _mapper.Map<Customer>(customerToAdd);
                _unitOfWork.Customer.Add(customer);
                await _unitOfWork.Complete();
                return CreatedAtAction(nameof(AddCustomer), customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetCustomerMoves/{id}")]
        [Authorize]
        public async Task<IActionResult> GetCustomerMoves(int id, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var customer = await _unitOfWork.Customer.GetFirstorDefault(c => c.Id == id);
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }
                var MovesList = _unitOfWork.Customer.GetCustomerMoves(id, fromDate, toDate);

                var customerToReturn = _mapper.Map<CustomerDisplayDto>(customer);

                return Ok(new { Customer = customerToReturn, MovesList = MovesList });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
