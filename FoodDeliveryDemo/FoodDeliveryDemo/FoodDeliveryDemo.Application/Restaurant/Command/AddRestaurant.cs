using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace FoodDeliveryDemo.Application.Restaurant.Command
{
	public class AddRestaurant : IRequest<bool>
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
	}
	public class AddRestaurantHandler : IRequestHandler<AddRestaurant, bool>
	{
		private readonly IConfiguration _config;

		public AddRestaurantHandler(IConfiguration config)
		{
            _config = config;
		}
		public async Task<bool> Handle(AddRestaurant request, CancellationToken cancellationToken)
		{
			try
			{
                int rowsAffected = 0;

                using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var queryforAddRestaurant = @"INSERT INTO [dbo].[Restaurant]
                                                ([RestaurantId]
                                                ,[Name]
                                                ,[Address]
                                                ,[PhoneNumber]
                                                ,[StartTime]
                                                ,[EndTime]
                                                ,RestaurantStatusId
                                                ,[CreatedOn]
                                                ,[CreatedBy])
                                            VALUES
                                                (@RestaurantId
                                                ,@Name
                                                ,@Address
                                                ,@PhoneNumber
                                                ,@StartTime
                                                ,@EndTime
                                                ,@RestaurantStatusId
                                                ,@CreatedOn
                                                ,'admin')";
                    DynamicParameters dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@RestaurantId", Guid.NewGuid());
                    dynamicParameters.Add("@Name", request.Name);
                    dynamicParameters.Add("@Address", request.Address);
                    dynamicParameters.Add("@PhoneNumber", request.PhoneNumber);
                    dynamicParameters.Add("@StartTime", request.StartTime);
                    dynamicParameters.Add("@EndTime", request.EndTime);
                    dynamicParameters.Add("@RestaurantStatusId", new Guid("51C1FBBE-3FAA-46DB-857B-A7AA7372A597"));
					dynamicParameters.Add("@CreatedOn", DateTime.Now);

					rowsAffected = await connection.ExecuteAsync(queryforAddRestaurant,dynamicParameters);


                    if(rowsAffected > 0)
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
