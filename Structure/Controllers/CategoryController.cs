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
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCategory(int id)
        {
            //Category category = _unitOfWork.Category.GetFirstorDefault(c => c.Id == id, IncludeWord: "Products");
            //return Ok(category);
            try
            {
                var category = await _unitOfWork.Category.GetFirstorDefault(c => c.Id == id);
                if (category == null)
                {
                    return NotFound("Category not found");
                }
                var categoryToReturn = _mapper.Map<CategoryDisplayDto>(category);
                return Ok(categoryToReturn);
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
                var categories = await _unitOfWork.Category.GetAll();
                var categoriesToReturn = _mapper.Map<IEnumerable<CategoryDisplayDto>>(categories);
                return Ok(categoriesToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = SD.AdminRole)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _unitOfWork.Category.GetFirstorDefault(c => c.Id == id);
                if (category == null)
                {
                    return NotFound("Category not found");
                }
                _unitOfWork.Category.Remove(category);
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
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryToUpdate)
        {
            try
            {
                if (id != categoryToUpdate.Id)
                {
                    return BadRequest("Category ID mismatch");
                }
                var existingCategory = await _unitOfWork.Category.GetFirstorDefault(c => c.Id == id);
                if (existingCategory == null)
                {
                    return NotFound("Category not found");
                }
                var category = _mapper.Map<Category>(categoryToUpdate);
                _unitOfWork.Category.Update(category);
                await _unitOfWork.Complete();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCategory(CategoryCreateDto categoryToAdd)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryToAdd);
                _unitOfWork.Category.Add(category);
                await _unitOfWork.Complete();
                return CreatedAtAction(nameof(AddCategory), category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
