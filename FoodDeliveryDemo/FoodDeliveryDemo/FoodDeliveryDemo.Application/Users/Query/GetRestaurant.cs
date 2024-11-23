using Dapper;
using FoodDeliveryDemo.Application.Restaurant.Query;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryDemo.Application.Users.Query
{
    public class GetRestaurant : IRequest<List<RestaurantVm>>
    {
        public string AvailableTime { get; set; }
    }
    public class GetRestaurantHandler : IRequestHandler<GetRestaurant, List<RestaurantVm>>
    {
        private readonly IConfiguration _configuration;

        public GetRestaurantHandler(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public async Task<List<RestaurantVm>> Handle(GetRestaurant request, CancellationToken cancellationToken)
        {
            try
            {
                List<RestaurantVm> result = null;

                using (var connection = new SqlConnection(_configuration.GetConnectionString("Defaultconnection")))
                {
                    connection.Open();
                    var query = @"select * from Restaurant where @AvailableTime Between StartTime and EndTime";
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@AvailableTime", request.AvailableTime);
                    result = (List<RestaurantVm>)await connection.QueryAsync<RestaurantVm>(query, parameters);

                    connection.Close();
                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
