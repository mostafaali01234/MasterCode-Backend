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
    public class LoanController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LoanController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetLoan(int id)
        {
            try
            {
                var loan = await _unitOfWork.Loan.GetFirstorDefault(c => c.Id == id, IncludeWord: "ApplicationUser,MoneySafe,Emp");
                if (loan == null)
                {
                    return NotFound("Loan not found");
                }
                var loanToReturn = _mapper.Map<LoanDisplayDto>(loan);
                return Ok(loanToReturn);
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
                var loans = await _unitOfWork.Loan.GetAll(IncludeWord: "ApplicationUser,MoneySafe,Emp");
                var loansToReturn = _mapper.Map<IEnumerable<LoanDisplayDto>>(loans);
                return Ok(loansToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = SD.AdminRole)]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            try
            {
                var loan = await _unitOfWork.Loan.GetFirstorDefault(c => c.Id == id);
                if (loan == null)
                {
                    return NotFound("Loan not found");
                }
                _unitOfWork.Loan.Remove(loan);
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
        public async Task<IActionResult> UpdateLoan(int id, [FromBody] LoanUpdateDto loanToUpdate)
        {
            try
            {
                if (id != loanToUpdate.Id)
                {
                    return BadRequest("Loan ID mismatch");
                }
                var existingLoan = await _unitOfWork.Loan.GetFirstorDefault(c => c.Id == id);
                if (existingLoan == null)
                {
                    return NotFound("Loan not found");
                }
                var loan = _mapper.Map<Loan>(loanToUpdate);
                _unitOfWork.Loan.Update(loan);
                await _unitOfWork.Complete();
                return Ok(loanToUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddLoan(LoanCreateDto loanToAdd)
        {
            try
            {
                var loan = _mapper.Map<Loan>(loanToAdd);
                loan.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _unitOfWork.Loan.Add(loan);
                await _unitOfWork.Complete();
                return CreatedAtAction(nameof(AddLoan), loan);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
