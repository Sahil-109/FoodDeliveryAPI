using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryDemo.Application.Restaurant.Command
{
	public class UpdateRestaurant : IRequest<bool>
	{
		public Guid RestaurantId { get; set; }
		public Guid MenuItemId { get; set; }
		public Guid MenuId { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Description { get; set; }
		public Guid RestaurantStatusId { get; set; }
	}
	public class UpdateRestaurantHandler : IRequestHandler<UpdateRestaurant, bool>
	{
		private readonly IConfiguration _config;

		public UpdateRestaurantHandler(IConfiguration config)
		{
			_config = config;
		}
		public async Task<bool> Handle(UpdateRestaurant request, CancellationToken cancellationToken)
		{
			try
			{
				int rowsAffected = 0;

				using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
				{
					connection.Open();
					var queryforUpdateMenuItems = @"UPDATE [dbo].[MenuItem]
													SET [MenuId] = @MenuId,
														[Name] = @Name,
														[Price] = @Price,
														[Description] = @Description,
														[ModifiedOn] = @ModifiedOn,
														[ModifiedBy] = @ModifiedBy
													WHERE [MenuItemId] = @MenuItemId";
					DynamicParameters dynamicParameters = new DynamicParameters();
					dynamicParameters.Add("@RestaurantId", Guid.NewGuid());
					dynamicParameters.Add("@MenuId", request.MenuId);
					dynamicParameters.Add("@MenuItemId", request.MenuItemId);
					dynamicParameters.Add("@Name", request.Name);
					dynamicParameters.Add("@Price", request.Price);
					dynamicParameters.Add("@Description", request.Description);
					dynamicParameters.Add("@ModifiedOn", DateTime.Now);
					dynamicParameters.Add("@ModifiedBy","admin");

					rowsAffected = await connection.ExecuteAsync(queryforUpdateMenuItems, dynamicParameters);

					if(rowsAffected > 0)
					{
						var queryforUpdateRestaurant = @"update Restaurant SET RestaurantStatusId = @RestaurantStatusId where RestaurantId = @RestaurantId";
						dynamicParameters = new DynamicParameters();
						dynamicParameters.Add("@RestaurantStatusId", request.RestaurantStatusId);
						dynamicParameters.Add("@RestaurantId", request.RestaurantId);

						rowsAffected = await connection.ExecuteAsync(queryforUpdateRestaurant, dynamicParameters);
					}


					if (rowsAffected > 0)
					{
						connection.Close();
						return true;
					}
					return false;

				}

			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}
	}
}