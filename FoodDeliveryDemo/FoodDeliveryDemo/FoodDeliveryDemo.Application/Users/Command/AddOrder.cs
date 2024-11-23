using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryDemo.Application.Users.Command
{
    public class AddOrder : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public Guid RestaurantId { get; set; }
        public Guid? DeliveryAgentId { get; set; }
        public Guid? OrderStatusId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }

        public class OrderDetail
        {
            public Guid MenuItemId { get; set; }
            public long Quantity { get; set; }
            public decimal Amount { get; set; }
        }
    }
    public class AddOrderHandler : IRequestHandler<AddOrder, bool>
    {
        private readonly IConfiguration _config;

        public AddOrderHandler(IConfiguration config)
        {
            _config = config;
        }
        public async Task<bool> Handle(AddOrder request, CancellationToken cancellationToken)
        {
            try
            {
                int rowsAffected = 0;

                var orderId = Guid.NewGuid();

                using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var queryforAddOrder = @"INSERT INTO [dbo].[Orders]
											([OrderId]
											,[UserId]
											,[RestaurantId]
											,[DeliveryAgentId]
											,[OrderStatusId]
											,[TotalAmount]
											,[CreatedOn]
											,[OrderDate])
										VALUES
											(@OrderId
											,@UserId
											,@RestaurantId
											,@DeliveryAgentId
											,@OrderStatusId
											,@TotalAmount
											,@CreatedOn
											,@OrderDate)";

                    DynamicParameters dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@OrderId", orderId);
                    dynamicParameters.Add("@UserId", request.UserId);
                    dynamicParameters.Add("@RestaurantId", request.RestaurantId);
                    dynamicParameters.Add("@DeliveryAgentId", request.DeliveryAgentId);
                    dynamicParameters.Add("@OrderStatusId", request.OrderStatusId);
                    dynamicParameters.Add("@TotalAmount", request.TotalAmount);
                    dynamicParameters.Add("@OrderDate", request.OrderDate.Date);
                    dynamicParameters.Add("@CreatedOn", DateTime.Now);

                    rowsAffected = await connection.ExecuteAsync(queryforAddOrder, dynamicParameters);

                    if (rowsAffected > 0)
                    {
                        foreach (var item in request.OrderDetails)
                        {
                            var queryForAddOrderDetails = @"INSERT INTO [dbo].[OrderDetails]
															([OrderDetailsId]
															,[OrderId]
															,[MenuItemId]
															,[Amount]
															,[CreatedOn]
															,[Quantity])
														VALUES
															(@OrderDetailsId
															,@OrderId
															,@MenuItemId
															,@Amount
															,@CreatedOn
															,@Quantity)";
                            dynamicParameters = new DynamicParameters();
                            dynamicParameters.Add("@OrderDetailsId", Guid.NewGuid());
                            dynamicParameters.Add("@OrderId", orderId);
                            dynamicParameters.Add("@MenuItemId", item.MenuItemId);
                            dynamicParameters.Add("@Amount", item.Amount);
                            dynamicParameters.Add("@Quantity", item.Quantity);
                            dynamicParameters.Add("@CreatedOn", DateTime.Now);

                            rowsAffected = await connection.ExecuteAsync(queryForAddOrderDetails, dynamicParameters);
                        }
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