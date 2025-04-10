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
    public class ShoppingCartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ShoppingCartController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetShoppingCart()
        {
            try
            {
                var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var cart = await _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == uid, IncludeWord: "Product");
                var cartToReturn = _mapper.Map<IEnumerable<ShoppingCartDisplayDto>>(cart);
                return Ok(cartToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFromShoppingCart(int id)
        {
            try
            {
                var old = await _unitOfWork.ShoppingCart.GetFirstorDefault(z => z.Id == id);
                if (old == null)
                {
                    return NotFound("ShoppingCart item not found");
                }
                _unitOfWork.ShoppingCart.Remove(old);
              
                await _unitOfWork.Complete();
                return await GetShoppingCart();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAllShoppingCart()
        {
            try
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var old = await _unitOfWork.ShoppingCart.GetAll(z => z.ApplicationUserId == user);
                if (old == null)
                {
                    return NotFound("ShoppingCart item not found");
                }
                _unitOfWork.ShoppingCart.RemoveRange(old);

                await _unitOfWork.Complete();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToShoppingCart(ShoppingCartCreateDto cartToAdd)
        {
            try
            {
                cartToAdd.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var cart = _mapper.Map<ShoppingCart>(cartToAdd);
                _unitOfWork.ShoppingCart.Add(cart);
                await _unitOfWork.Complete();
                return await GetShoppingCart();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
