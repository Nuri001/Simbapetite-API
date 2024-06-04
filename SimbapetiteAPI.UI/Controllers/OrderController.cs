using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simbapetite.Core.Domain.Entities;
using Simbapetite.Core.DTO;
using Simbapetite.Core.Helpers;
using Simbapetite.Core.Services;
using Simbapetite.Core.ServicesContracts;
using Stripe.Climate;
using System.Net;

namespace Simbapetite.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private ApiResponse _response;
		
		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
			_response = new ApiResponse();

		}

		[HttpGet]
		public async Task<IActionResult> GetOrders()
		{

			_response.Result = await _orderService.GetAllOrderHeaders();
			_response.StatusCode = HttpStatusCode.OK;
			return Ok(_response);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<ApiResponse>> GetOrders(int id)
		{
			try
			{
				if (id == 0)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}


				var orderHeaders = await _orderService.GetOrders(id);
				if (orderHeaders == null)
				{
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				_response.Result = orderHeaders;
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpPost]
		public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDTO orderHeaderDTO)
		{
			try
			{
				if (ModelState.IsValid)
				{
					OrderHeader order= await _orderService.CreatOrder(orderHeaderDTO);
					_response.Result = order;
					order.OrderDetails = null;
					_response.StatusCode = HttpStatusCode.Created;
					return Ok(_response);
				}
			

			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<ApiResponse>> UpdateOrderHeader(int id, [FromBody] OrderHeaderUpdateDTO orderHeaderUpdateDTO)
		{
			try
			{
				if (orderHeaderUpdateDTO == null || id != orderHeaderUpdateDTO.OrderHeaderId)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest();
				}

				_response.Result =await _orderService.UpdateOrderHeader(id, orderHeaderUpdateDTO);
				_response.StatusCode = HttpStatusCode.NoContent;
				_response.IsSuccess = true;
				return Ok(_response);



			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
			}
			return _response;
		}
	}
}
