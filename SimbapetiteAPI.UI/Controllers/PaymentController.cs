using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simbapetite.Core.Domain.Entities;
using Simbapetite.Core.DTO;
using Simbapetite.Core.ServicesContracts;
using Stripe;
using System.Net;

namespace Simbapetite_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		protected ApiResponse _response;
		private readonly IConfiguration _congifuration;
		private readonly IShoppingCartService _shoppingCartService;
		public PaymentController(IConfiguration configuration, IShoppingCartService shoppingCartService)
		{
			_congifuration = configuration;
			_shoppingCartService = shoppingCartService;
			_response = new();
		}

		[HttpPost]
		public async Task<ActionResult<ApiResponse>> MakePayment(string userId)
		{
			ShoppingCart shoppingCart = await _shoppingCartService.GetShoppingCart(userId);

			if (shoppingCart == null || shoppingCart.CartItems == null || shoppingCart.CartItems.Count() == 0)
			{
				_response.StatusCode = System.Net.HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				return BadRequest(_response);
			}

			#region Create Payment Intent

			StripeConfiguration.ApiKey = _congifuration["StripeSettings:SecretKey"];
			shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);

			PaymentIntentCreateOptions options = new()
			{
				Amount = (int)(shoppingCart.CartTotal * 100),
				Currency = "usd",
				PaymentMethodTypes = new List<string>
				  {
					"card",
				  },
			};
			PaymentIntentService service = new();
			PaymentIntent response = service.Create(options);
			shoppingCart.StripePaymentIntentId = response.Id;
			shoppingCart.ClientSecret = response.ClientSecret;


			#endregion

			_response.Result = shoppingCart;
			_response.StatusCode = HttpStatusCode.OK;
			return Ok(_response);
		}

	}
}
