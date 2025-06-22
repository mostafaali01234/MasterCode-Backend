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
    public class JobController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public JobController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetJob(int id)
        {
            try
            {
                var job = await _unitOfWork.Job.GetFirstorDefault(c => c.Id == id);
                if (job == null)
                {
                    return NotFound("Job not found");
                }
                var jobToReturn = _mapper.Map<JobDisplayDto>(job);
                return Ok(jobToReturn);
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
                var jobs = await _unitOfWork.Job.GetAll();
                var jobsToReturn = _mapper.Map<IEnumerable<JobDisplayDto>>(jobs);
                return Ok(jobsToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = SD.AdminRole)]
        public async Task<IActionResult> DeleteJob(int id)
        {
            try
            {
                var job = await _unitOfWork.Job.GetFirstorDefault(c => c.Id == id);
                if (job == null)
                {
                    return NotFound("Job not found");
                }
                _unitOfWork.Job.Remove(job);
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
        public async Task<IActionResult> UpdateJob(int id, [FromBody] JobUpdateDto jobToUpdate)
        {
            try
            {
                if (id != jobToUpdate.Id)
                {
                    return BadRequest("Job ID mismatch");
                }
                var existingJob = await _unitOfWork.Job.GetFirstorDefault(c => c.Id == id);
                if (existingJob == null)
                {
                    return NotFound("Job not found");
                }
                var job = _mapper.Map<Job>(jobToUpdate);
                _unitOfWork.Job.Update(job);
                await _unitOfWork.Complete();
                return Ok(jobToUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddJob(CategoryCreateDto jobToAdd)
        {
            try
            {
                var job = _mapper.Map<Job>(jobToAdd);
                _unitOfWork.Job.Add(job);
                await _unitOfWork.Complete();
                return CreatedAtAction(nameof(AddJob), job);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
