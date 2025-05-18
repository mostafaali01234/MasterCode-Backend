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
    public class MoneySafeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MoneySafeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetMoneySafe(int id)
        {
            try
            {
                var moneySafe = await _unitOfWork.MoneySafe.GetFirstorDefault(c => c.Id == id, IncludeWord: "ApplicationUser");
                if (moneySafe == null)
                {
                    return NotFound("MoneySafe not found");
                }
                var moneySafeToReturn = _mapper.Map<MoneySafeDisplayDto>(moneySafe);
                moneySafeToReturn.CurrentBalance = _unitOfWork.MoneySafe.GetCurrentBalance(id);
                return Ok(moneySafeToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetCurrentBalance/{id}")]
        [Authorize]
        public async Task<IActionResult> GetCurrentBalance(int id)
        {
            try
            {
                var moneySafe = await _unitOfWork.MoneySafe.GetFirstorDefault(c => c.Id == id, IncludeWord: "ApplicationUser");
                if (moneySafe == null)
                {
                    return NotFound("MoneySafe not found");
                }
                var balance = _unitOfWork.MoneySafe.GetCurrentBalance(id);
                var moneySafeToReturn = _mapper.Map<MoneySafeDisplayDto>(moneySafe);
                return Ok(new { Balance = balance, MoneySafe = moneySafeToReturn });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("GetMoneysafeMoves/{id}")]
        [Authorize]
        public async Task<IActionResult> GetMoneysafeMoves(int id, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var moneySafe = await _unitOfWork.MoneySafe.GetFirstorDefault(c => c.Id == id, IncludeWord: "ApplicationUser");
                if (moneySafe == null)
                {
                    return NotFound("MoneySafe not found");
                }
                var MovesList = _unitOfWork.MoneySafe.GetMoneysafeMoves(id, fromDate, toDate);

                var moneySafeToReturn = _mapper.Map<MoneySafeDisplayDto>(moneySafe);

                return Ok(new { MoneySafe = moneySafeToReturn, MovesList = MovesList });
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
                var moneysafes = await _unitOfWork.MoneySafe.GetAll(IncludeWord: "ApplicationUser");
                var moneysafesToReturn = _mapper.Map<IEnumerable<MoneySafeDisplayDto>>(moneysafes);
                foreach (var ms in moneysafesToReturn)
                    ms.CurrentBalance = _unitOfWork.MoneySafe.GetCurrentBalance(ms.Id);
                return Ok(moneysafesToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = SD.AdminRole)]
        public async Task<IActionResult> DeleteMoneySafe(int id)
        {
            try
            {
                var moneysafe = await _unitOfWork.MoneySafe.GetFirstorDefault(c => c.Id == id);
                if (moneysafe == null)
                {
                    return NotFound("MoneySafe not found");
                }
                _unitOfWork.MoneySafe.Remove(moneysafe);
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
        public async Task<IActionResult> UpdateMoneySafe(int id, [FromBody] MoneySafeUpdateDto moneysafeToUpdate)
        {
            try
            {
                if (id != moneysafeToUpdate.Id)
                {
                    return BadRequest("MoneySafe ID mismatch");
                }
                var existingmoneysafe = await _unitOfWork.MoneySafe.GetFirstorDefault(c => c.Id == id);
                if (existingmoneysafe == null)
                {
                    return NotFound("MoneySafe not found");
                }
                var moneysafe = _mapper.Map<MoneySafe>(moneysafeToUpdate);
                _unitOfWork.MoneySafe.Update(moneysafe);
                await _unitOfWork.Complete();
                return Ok(moneysafeToUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddMoneySafe(MoneySafeCreateDto moneysafeToAdd)
        {
            try
            {
                var moneysafe = _mapper.Map<MoneySafe>(moneysafeToAdd);
                _unitOfWork.MoneySafe.Add(moneysafe);
                await _unitOfWork.Complete();
                return CreatedAtAction(nameof(AddMoneySafe), moneysafe);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
