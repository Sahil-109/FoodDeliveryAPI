using Dapper;
using FoodDeliveryDemo.Application.Restaurant.Command;
using FoodDeliveryDemo.Application.Restaurant.Query;
using FoodDeliveryDemo.Application.Users.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TrackerAdminApi.DataModel;

namespace FoodDeliveryDemo.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class RestaurantController : ControllerBase
	{
		private readonly ILogger<RestaurantController> _logger;
		private readonly IMediator _mediator;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IConfiguration _configuration;
		public RestaurantController(ILogger<RestaurantController> logger, IMediator mediator, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
		{
			_logger = logger;
			_mediator = mediator;
			_webHostEnvironment = webHostEnvironment;
			_configuration = configuration;
		}

		[HttpPost]
		public async Task<IActionResult> AddRestaurant(AddRestaurant reqParams)
		{
			ResponseBase<bool> responseBase = new ResponseBase<bool>();
			try
			{
				var result = await _mediator.Send(reqParams);

				if (result is true)
				{
					responseBase.Data = result;
					responseBase.Code = StatusCodes.Status200OK;
					responseBase.Message = "Add Restaurant Successfully";
					return new OkObjectResult(responseBase);
				}
				else
				{
					responseBase.Data = responseBase.Data;
					responseBase.Code = StatusCodes.Status400BadRequest;
					responseBase.IsSuccessful = false;
					responseBase.Message = "Data failed to Add";
					return new BadRequestObjectResult(responseBase);
				}
			}
			catch (Exception ex)
			{
				_logger.LogInformation("Error while Adding " + ex.Message);
				responseBase.Code = StatusCodes.Status400BadRequest;
				responseBase.IsSuccessful = false;
				responseBase.Message = ex.Message;
				return new BadRequestObjectResult(responseBase);
			}
		}

		[HttpPost]
		public async Task<IActionResult> UpdateRestaurant(UpdateRestaurant reqParams)
		{
			ResponseBase<bool> responseBase = new ResponseBase<bool>();
			try
			{
				var result = await _mediator.Send(reqParams);

				if (result is true)
				{
					responseBase.Data = result;
					responseBase.Code = StatusCodes.Status200OK;
					responseBase.Message = "Update Restaurant Successfully";
					return new OkObjectResult(responseBase);
				}
				else
				{
					responseBase.Data = responseBase.Data;
					responseBase.Code = StatusCodes.Status400BadRequest;
					responseBase.IsSuccessful = false;
					responseBase.Message = "Data failed to Update";
					return new BadRequestObjectResult(responseBase);
				}
			}
			catch (Exception ex)
			{
				_logger.LogInformation("Error while Updating " + ex.Message);
				responseBase.Code = StatusCodes.Status400BadRequest;
				responseBase.IsSuccessful = false;
				responseBase.Message = ex.Message;
				return new BadRequestObjectResult(responseBase);
			}
		}
		

		[HttpPost]
		public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatus reqParams)
		{
			ResponseBase<bool> responseBase = new ResponseBase<bool>();
			try
			{
				var result = await _mediator.Send(reqParams);

				if (result is true)
				{
					responseBase.Data = result;
					responseBase.Code = StatusCodes.Status200OK;
					responseBase.Message = "Update OrderStatus Successfully";
					return new OkObjectResult(responseBase);
				}
				else
				{
					responseBase.Data = responseBase.Data;
					responseBase.Code = StatusCodes.Status400BadRequest;
					responseBase.IsSuccessful = false;
					responseBase.Message = "Data failed to Update";
					return new BadRequestObjectResult(responseBase);
				}
			}
			catch (Exception ex)
			{
				_logger.LogInformation("Error while Updating " + ex.Message);
				responseBase.Code = StatusCodes.Status400BadRequest;
				responseBase.IsSuccessful = false;
				responseBase.Message = ex.Message;
				return new BadRequestObjectResult(responseBase);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetOrders(Guid? OrderId)
		{
			ResponseBase<RestaurantVm> responseBase = new ResponseBase<RestaurantVm>();
			try
			{
				var result = await _mediator.Send(new GetOrders { OrderId = OrderId });

				if (result != null)
				{
					responseBase.Data = result;
					responseBase.Code = StatusCodes.Status200OK;
					responseBase.Message = "Get Orders Successfully";
					return new OkObjectResult(responseBase);
				}
				else
				{
					responseBase.Data = responseBase.Data;
					responseBase.Code = StatusCodes.Status400BadRequest;
					responseBase.IsSuccessful = false;
					responseBase.Message = "Data failed to Get";
					return new BadRequestObjectResult(responseBase);
				}
			}
			catch (Exception ex)
			{
				_logger.LogInformation("Error while Getting " + ex.Message);
				responseBase.Code = StatusCodes.Status400BadRequest;
				responseBase.IsSuccessful = false;
				responseBase.Message = ex.Message;
				return new BadRequestObjectResult(responseBase);
			}
		}

	}
}
