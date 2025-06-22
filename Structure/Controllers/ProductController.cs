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
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var Product = await _unitOfWork.Product.GetFirstorDefault(c => c.Id == id, IncludeWord: "Category");
                if (Product == null)
                {
                    return NotFound("Product not found");
                }
                var ProductToReturn = _mapper.Map<ProductDisplayDto>(Product);
                return Ok(ProductToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getProductByName")]
        [Authorize]
        public async Task<IActionResult> GetProductByName(string searchText)
        {
            try
            {
                var Product = await _unitOfWork.Product.GetFirstorDefault(c => c.Title.Contains(searchText), IncludeWord: "Category");
                if (Product == null)
                {
                    return NotFound("Product not found");
                }
                var ProductToReturn = _mapper.Map<ProductDisplayDto>(Product);
                return Ok(ProductToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(int? catId)
        {
            try
            {
                var products = await _unitOfWork.Product.GetAll(z => catId == null || catId == 0 || z.CategoryId == catId ,IncludeWord: "Category");
                var productsToReturn = _mapper.Map<IEnumerable<ProductDisplayDto>>(products);
                return Ok(productsToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = SD.AdminRole)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.Product.GetFirstorDefault(c => c.Id == id);
                if (product == null)
                {
                    return NotFound("Product not found");
                }
                _unitOfWork.Product.Remove(product);
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
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto productToUpdate)
        {
            try
            {
                if (id != productToUpdate.Id)
                {
                    return BadRequest("Product ID mismatch");
                }
                var existingProduct = await _unitOfWork.Product.GetFirstorDefault(c => c.Id == id);
                if (existingProduct == null)
                {
                    return NotFound("Product not found");
                }
                var product = _mapper.Map<Product>(productToUpdate);
                _unitOfWork.Product.Update(product);
                await _unitOfWork.Complete();
                return Ok(productToUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProduct(ProductCreateDto productToAdd)
        {
            try
            {
                var product = _mapper.Map<Product>(productToAdd);
                _unitOfWork.Product.Add(product);
                await _unitOfWork.Complete();
                return CreatedAtAction(nameof(AddProduct), product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
