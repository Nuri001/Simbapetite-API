using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simbapetite.Core.Domain.Entities;
using Simbapetite.Core.DTO;
using Simbapetite.Core.ServicesContracts;
using System.Net;

namespace Simbapetite_API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ShoppingCartController : ControllerBase
	{
		protected ApiResponse _response;
		private readonly IShoppingCartService _shoppingCartService;
	
		public ShoppingCartController(IShoppingCartService shoppingCartService)
		{
			_response = new();
			_shoppingCartService= shoppingCartService;
	
		}

		[HttpGet]
		public async Task<ActionResult<ApiResponse>> GetShoppingCart(string userId)
		{
			try
			{
				ShoppingCart shoppingCart= await _shoppingCartService.GetShoppingCart(userId);
				
				_response.Result = shoppingCart;
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
				_response.StatusCode = HttpStatusCode.BadRequest;
			}
			return _response;

		}



		[HttpPost]
		public async Task<ActionResult<ApiResponse>> AddOrUpdateItemInCart(string userId, int menuItemId, int updateQuantityBy)
		{
			
			try
			{
				bool success = await _shoppingCartService.AddOrUpdateItemInCart(userId, menuItemId, updateQuantityBy);
				if (!success) {
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					return BadRequest(_response);

				}
				
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
				_response.StatusCode = HttpStatusCode.BadRequest;
			}
			return _response;
		}
	}
}
