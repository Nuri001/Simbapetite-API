using Azure;
using Microsoft.AspNetCore.Mvc.Filters;
using Simbapetite.Core.DTO;
using Simbapetite.UI.Controllers;
using System.Net;

using Microsoft.AspNetCore.Mvc;



using System.Linq;

namespace Simbapetite.UI.Filters.ActionFilters
{
    public class MenuItemCreatPostActionFilter : IAsyncActionFilter
	{
		private readonly ILogger<MenuItemCreatPostActionFilter> _logger;

		public MenuItemCreatPostActionFilter(ILogger<MenuItemCreatPostActionFilter> logger)
		{
			_logger = logger;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			_logger.LogInformation("In befor logic of MenuItemCreatAndUpdatePostActionFilter Action filter");
			ApiResponse response = new ApiResponse();
			if (context.Controller is MenuItemController menuItemController)
			{
				if (menuItemController.ModelState.IsValid)
				{
					try
					{
						MenuItemCreateDTO menuItemCreateDTO = (MenuItemCreateDTO)context.ActionArguments["menuItemCreateDTO"];
						if (menuItemCreateDTO.File == null || menuItemCreateDTO.File.Length == 0)
						{

							response.StatusCode = HttpStatusCode.BadRequest;
							response.IsSuccess = false;
							response.ErrorMessages = new List<string>() { 
							"No Valid Image File"
							};
							context.Result = new BadRequestObjectResult(response);
							return;
						}
						else
						{
							await next();
						}

					}
					catch (Exception ex)
					{
						response.IsSuccess = false;
						response.ErrorMessages
							 = new List<string>() { ex.ToString() };
						context.Result = new ObjectResult(response);
						return;
					}

					await next();
				}
				else
				{
					context.Result = new BadRequestObjectResult("ModelState Is Not Valid");
					return;
				}
			}
			else
			{
				await next();
			}
			_logger.LogInformation("In after logic of MenuItemCreatAndUpdatePostActionFilter Action filter");
		}
	}
}
