using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using static Dapper.SqlMapper;
using static FoodDeliveryDemo.Application.Restaurant.Query.OrdersVm;

namespace FoodDeliveryDemo.Application.Restaurant.Query
{
	public class GetOrders : IRequest<List<OrdersVm>>
	{
		public Guid? OrderId { get; set; }
	}
	public class GetOrdersHandler : IRequestHandler<GetOrders, List<OrdersVm>>
	{
		private readonly IConfiguration _configuration;

		public GetOrdersHandler(IConfiguration configuration)
		{
			_configuration = configuration;

		}

		public async Task<List<OrdersVm>> Handle(GetOrders request, CancellationToken cancellationToken)
		{
			try
			{
				OrdersVm AvailableDeliveryAgents = null;
				List<OrderDetail> orderDetails = null;

				using (var connection = new SqlConnection(_configuration.GetConnectionString("Defaultconnection")))
				{
					DynamicParameters dynamicParameters = new DynamicParameters();


					connection.Open();
					var query = @"SELECT o.*,r.Name AS RestaurantName,da.FirstName AS DeliveryAgentName,os.OrderStatus,da.DeliveryAgentId,da.FirstName + ' ' + da.LastName AS		          DeliveryAgentName,u.Username
								FROM Orders o 
								JOIN Restaurant r ON r.RestaurantId = o.RestaurantId 
								JOIN Users u ON u.UserId = o.UserId
								LEFT JOIN DeliveryAgent da ON da.DeliveryAgentId = o.DeliveryAgentId 
								LEFT JOIN OrderStatus os ON os.OrderStatusId = o.OrderStatusId";

					if (request.OrderId != null)
					{
						query += @" where o.OrderId = @OrderId";
						dynamicParameters.Add("@OrderId", request.OrderId);
					}
					query += @" ORDER BY o.CreatedOn DESC";

					var result = await connection.QueryAsync<OrdersVm>(query, dynamicParameters);

					var queryforOrderDetails = @"select od.*,m.Name as MenuItemName from OrderDetails od JOIN MenuItem m ON m.MenuItemId = od.MenuItemId where od.OrderId = @OrderId";
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("@OrderId", request.OrderId);
					orderDetails = (List<OrderDetail>)await connection.QueryAsync<OrderDetail>(queryforOrderDetails, parameters);

					var queryForCheckAvailableDeliveryAgents = @"select * from DeliveryAgent where Status = 'Available'";
					AvailableDeliveryAgents = await connection.QueryFirstAsync<OrdersVm>(queryForCheckAvailableDeliveryAgents);

					var queryForAgentsOrders = @"select * from Orders where DeliveryAgentId IS NULL and OrderStatusId = 'CC4FD810-A188-4A7B-BF14-480ABC02E214'";
					var NullDAgentsOrders = await connection.QueryFirstOrDefaultAsync<OrdersVm>(queryForAgentsOrders);

					if (AvailableDeliveryAgents != null && NullDAgentsOrders != null && request.OrderId == null)
					{
						var updateQueryForOrders = @"Update Orders SET DeliveryAgentId = @DeliveryAgentId where OrderId = @OrderId";
						DynamicParameters dynamic = new DynamicParameters();
						dynamic.Add("@DeliveryAgentId", AvailableDeliveryAgents.DeliveryAgentId);
						dynamic.Add("@OrderId", NullDAgentsOrders.OrderId);
						await connection.ExecuteAsync(updateQueryForOrders, dynamic);

						var updateDAgentStatus = @"Update DeliveryAgent SET Status = 'Not Available' where DeliveryAgentId = @DeliveryAgentId";
						dynamic = new DynamicParameters();
						dynamic.Add("@DeliveryAgentId", AvailableDeliveryAgents.DeliveryAgentId);
						await connection.ExecuteAsync(updateDAgentStatus, dynamic);
					}

					var responseList = result.Select(order => new OrdersVm
					{
						DeliveryAgentId = order.DeliveryAgentId,
						DeliveryAgentName = order.DeliveryAgentName,
						OrderDate = order.OrderDate,
						OrderId = order.OrderId,
						OrderStatusId = order.OrderStatusId,
						OrderStatus = order.OrderStatus,
						RestaurantId = order.RestaurantId,
						RestaurantName = order.RestaurantName,
						TotalAmount = order.TotalAmount,
						UserId = order.UserId,
						UserName = order.UserName,
						OrderDetails = orderDetails.ToList()
					}).ToList();

					connection.Close();
					return responseList;

				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}