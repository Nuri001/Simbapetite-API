using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Simbapetite.Core.DTO;
using Simbapetite.UI.Controllers;
using System.Net;

namespace Simbapetite.UI.Filters.ActionFilters
{
    public class MenuItemUpdatePostActionFilter : IAsyncActionFilter
	{
		private readonly ILogger<MenuItemCreatPostActionFilter> _logger;

		public MenuItemUpdatePostActionFilter(ILogger<MenuItemCreatPostActionFilter> logger)
		{
			_logger = logger;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			_logger.LogInformation("In befor logic of MenuItemUpdatePostActionFilter Action filter");
			ApiResponse response = new ApiResponse();
			if (context.Controller is MenuItemController menuItemController)
			{
				if (menuItemController.ModelState.IsValid)
				{

					MenuItemUpdateDTO menuItemUpdateDTO = (MenuItemUpdateDTO)context.ActionArguments["menuItemUpdateDTO"];
					int id = (int)context.ActionArguments["id"];
					if (id != menuItemUpdateDTO.Id)
					{

						response.StatusCode = HttpStatusCode.BadRequest;
						response.IsSuccess = false;
						response.ErrorMessages = new List<string>() {
							"id!=menuItemUpdateDTO.Id"
							};
						context.Result = new BadRequestObjectResult(response);
						return;
					}
					else
					{
						await next();
					}


					await next();
				}
				else {
					context.Result = new BadRequestObjectResult("ModelState Is Not Valid");
					return;
				}
			}
			else
			{
				await next();
			}
			_logger.LogInformation("In after logic of MenuItemUpdatePostActionFilter Action filter");
		}
	}
}
