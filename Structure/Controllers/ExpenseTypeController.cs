using DataAccess.Repository;
using Entities.DTO;
using Entities.IRepository;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Utilities;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExpenseTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetExpenseType(int id)
        {
            try
            {
                var expenseType = await _unitOfWork.ExpenseType.GetFirstorDefault(c => c.Id == id);
                if (expenseType == null)
                {
                    return NotFound("ExpenseType not found");
                }
               
                return Ok(expenseType);
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
                var expensesTypes = await _unitOfWork.ExpenseType.GetAll();
                
                return Ok(expensesTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = SD.AdminRole)]
        public async Task<IActionResult> DeleteExpenseType(int id)
        {
            try
            {
                var expenseType = await _unitOfWork.ExpenseType.GetFirstorDefault(c => c.Id == id);
                if (expenseType == null)
                {
                    return NotFound("ExpenseType not found");
                }
                _unitOfWork.ExpenseType.Remove(expenseType);
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
        public async Task<IActionResult> UpdateExpenseType(int id, [FromBody] ExpenseType expenseTypeToUpdate)
        {
            try
            {
                if (id != expenseTypeToUpdate.Id)
                {
                    return BadRequest("ExpenseType ID mismatch");
                }
                var existingExpenseType = await _unitOfWork.ExpenseType.GetFirstorDefault(c => c.Id == id);
                if (existingExpenseType == null)
                {
                    return NotFound("Expense not found");
                }
                
                _unitOfWork.ExpenseType.Update(expenseTypeToUpdate);
                await _unitOfWork.Complete();
                return Ok(expenseTypeToUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddExpenseType(ExpenseType expenseTypeToAdd)
        {
            try
            {
                _unitOfWork.ExpenseType.Add(expenseTypeToAdd);
                await _unitOfWork.Complete();
                return CreatedAtAction(nameof(AddExpenseType), expenseTypeToAdd);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
