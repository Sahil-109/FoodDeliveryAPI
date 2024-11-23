using Dapper;
using FoodDeliveryDemo.Application.Restaurant.Query;
using FoodDeliveryDemo.Application.Users.Command;
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
	public class UsersController : ControllerBase
	{
		private readonly ILogger<RestaurantController> _logger;
		private readonly IMediator _mediator;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IConfiguration _configuration;
		public UsersController(ILogger<RestaurantController> logger, IMediator mediator, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
		{
			_logger = logger;
			_mediator = mediator;
			_webHostEnvironment = webHostEnvironment;
			_configuration = configuration;
		}

		[HttpGet]
		public async Task<IActionResult> GetRestaurant(string AvailableTime)
		{
			ResponseBase<RestaurantVm> responseBase = new ResponseBase<RestaurantVm>();
			try
			{
				var result = await _mediator.Send(new GetRestaurant { AvailableTime = AvailableTime });

				if (result != null)
				{
					responseBase.Data = result;
					responseBase.Code = StatusCodes.Status200OK;
					responseBase.Message = "Get Restaurant Successfully";
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

		[HttpPost]
		public async Task<IActionResult> AddOrder(AddOrder reqParams)
		{
			ResponseBase<bool> responseBase = new ResponseBase<bool>();
			try
			{
				var result = await _mediator.Send(reqParams);

				if (result is true)
				{
					responseBase.Data = result;
					responseBase.Code = StatusCodes.Status200OK;
					responseBase.Message = "Add Order Successfully";
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
		public IActionResult AddOrderRating(OrderRatingVm reqParams)
		{
			try
			{
				using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
				{
					connection.Open();
					string query = @"INSERT INTO OrderRating (OrderId, UserId, Rating, Comment) 
                             VALUES (@OrderId, @UserId, @Rating, @Comment)";
					connection.Execute(query, new { OrderId = reqParams.OrderId, UserId = reqParams.UserId, Rating = reqParams.Rating, Comment = reqParams.Comment });

					return Ok("Add OrderRating Successfully");
				}
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}

		}

		[HttpPost]
		public IActionResult AddDeliveryAgentRating(DeliveryAgentRatingVm reqParams)
		{
			try
			{
				using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
				{
					connection.Open();
					string query = @"INSERT INTO DeliveryAgentRating (DeliveryAgentId, UserId, Rating, Comment) 
                             VALUES (@DeliveryAgentId, @UserId, @Rating, @Comment)";
					connection.Execute(query, new { DeliveryAgentId = reqParams.DeliveryAgentId, UserId = reqParams.UserId, Rating = reqParams.Rating, Comment = reqParams.Comment });

					return Ok("Add DeliveryAgentRating Successfully");
				}
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}

		}
	}
}
