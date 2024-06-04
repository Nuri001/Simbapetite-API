using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simbapetite.Core.Domain.Entities;
using Simbapetite.Core.DTO;
using Simbapetite.Core.Helpers;
using Simbapetite.Core.ServicesContracts;
using Simbapetite.Infrastructure.DbContext;
using Simbapetite.UI.Filters.ActionFilters;
using System.Net;

namespace Simbapetite.UI.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class MenuItemController : ControllerBase
	{
		private readonly IMenuItemService _menuItemService;
		private ApiResponse _response;
		public MenuItemController(IMenuItemService menuItemService)
		{
			_menuItemService = menuItemService;
			_response = new ApiResponse();
		}

		[HttpGet]
		public async Task<IActionResult> GetMenuItems()
		{

			_response.Result = await _menuItemService.GetAllMenuItems();
			_response.StatusCode = HttpStatusCode.OK;
			return Ok(_response);
		}
		[HttpGet("{id:int}", Name = "GetMenuItem")]
		public async Task<IActionResult> GetMenuItem(int id)
		{
			if (id == 0)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				return BadRequest(_response);
			}
			MenuItem menuItem = await _menuItemService.GetMenuItem(id);
			if (menuItem == null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.IsSuccess = false;
				return NotFound(_response);
			}
			_response.Result = menuItem;
			_response.StatusCode = HttpStatusCode.OK;
			return Ok(_response);
		}
		[HttpPost]
		[TypeFilter(typeof(MenuItemCreatPostActionFilter))]
		public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm] MenuItemCreateDTO menuItemCreateDTO)
		{

			MenuItem createdMenuItem = await _menuItemService.CreateMenuItem(menuItemCreateDTO);
			_response.Result = createdMenuItem;
			_response.IsSuccess=true;
			_response.StatusCode = HttpStatusCode.Created;
			return CreatedAtRoute("GetMenuItem", new { id = createdMenuItem.Id }, _response);

		}

		[HttpPut("{id:int}")]
		[TypeFilter(typeof(MenuItemUpdatePostActionFilter))]
		public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id, [FromForm] MenuItemUpdateDTO menuItemUpdateDTO)
		{
			try
			{
				MenuItem updatedMenuItem=await _menuItemService.UpdateMenuItem(id, menuItemUpdateDTO);
				if (updatedMenuItem == null)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					return BadRequest();
				}
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.NoContent;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
				return _response;
			}

			
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult<ApiResponse>> DeleteMenuItem(int id)
		{
			try
			{
				if (await _menuItemService.DeleteMenuItem(id))
				{
					return Ok(_response);
				}
				else {
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					return BadRequest();
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

	}
}
