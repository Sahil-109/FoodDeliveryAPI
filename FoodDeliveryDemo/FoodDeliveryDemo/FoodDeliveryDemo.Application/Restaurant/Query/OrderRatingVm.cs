using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryDemo.Application.Restaurant.Query
{
    public class OrderRatingVm
    {
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

    }
}
