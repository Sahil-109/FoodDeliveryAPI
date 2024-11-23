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
	public class UpdateOrderStatus : IRequest<bool>
	{
		public Guid OrderStatusId { get; set; }
		public Guid OrderId { get; set; }
		public Guid? DeliveryAgentId { get; set; }
	}
	public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatus, bool>
	{
		private readonly IConfiguration _config;

		public UpdateOrderStatusHandler(IConfiguration config)
		{
			_config = config;
		}
		public async Task<bool> Handle(UpdateOrderStatus request, CancellationToken cancellationToken)
		{
			try
			{
				int rowsAffected = 0;

				using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
				{
					connection.Open();

					DynamicParameters dynamicParameters = new DynamicParameters();
					var queryforUpdateOrderStatus = @"update Orders SET OrderStatusId = @OrderStatusId where OrderId = @OrderId";
					dynamicParameters = new DynamicParameters();
					dynamicParameters.Add("@OrderStatusId", request.OrderStatusId);
					dynamicParameters.Add("@OrderId", request.OrderId);
					rowsAffected = await connection.ExecuteAsync(queryforUpdateOrderStatus, dynamicParameters);

					if (request.OrderStatusId == new Guid("3B3D0FF9-8C25-40DD-94AF-39A182737F27"))
					{
						var updateDAgentStatus = @"Update DeliveryAgent SET Status = 'Available' where DeliveryAgentId = @DeliveryAgentId";
						dynamicParameters = new DynamicParameters();
						dynamicParameters.Add("@DeliveryAgentId", request.DeliveryAgentId);
						await connection.ExecuteAsync(updateDAgentStatus, dynamicParameters);
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
