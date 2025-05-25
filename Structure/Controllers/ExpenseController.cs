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
    public class ExpenseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExpenseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetExpense(int id)
        {
            try
            {
                var expense = await _unitOfWork.Expense.GetFirstorDefault(c => c.Id == id, IncludeWord: "ExpenseType,Emp,ApplicationUser,MoneySafe");
                if (expense == null)
                {
                    return NotFound("Expense not found");
                }
                var expenseToReturn = _mapper.Map<ExpenseDisplayDto>(expense);
                return Ok(expenseToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(DateTime? fromDate, DateTime? toDate, string selectedEmpId, int selectedExpenseTypeId)
        {
            try
            {
                var expenses = await _unitOfWork.Expense.GetAllExpenses(fromDate, toDate, selectedEmpId, selectedExpenseTypeId);
                var expensesToReturn = _mapper.Map<IEnumerable<ExpenseDisplayDto>>(expenses);
                return Ok(expensesToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = SD.AdminRole)]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                var expense = await _unitOfWork.Expense.GetFirstorDefault(c => c.Id == id);
                if (expense == null)
                {
                    return NotFound("Expense not found");
                }
                _unitOfWork.Expense.Remove(expense);
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
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseUpdateDto expenseToUpdate)
        {
            try
            {
                if (id != expenseToUpdate.Id)
                {
                    return BadRequest("Expense ID mismatch");
                }
                var existingExpense = await _unitOfWork.Expense.GetFirstorDefault(c => c.Id == id);
                if (existingExpense == null)
                {
                    return NotFound("Expense not found");
                }
                var expense = _mapper.Map<Expense>(expenseToUpdate);
                _unitOfWork.Expense.Update(expense);
                await _unitOfWork.Complete();
                return Ok(expenseToUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddExpense(ExpenseCreateDto expenseToAdd)
        {
            try
            {
                var expense = _mapper.Map<Expense>(expenseToAdd);
                expense.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _unitOfWork.Expense.Add(expense);
                await _unitOfWork.Complete();
                return CreatedAtAction(nameof(AddExpense), expense);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
