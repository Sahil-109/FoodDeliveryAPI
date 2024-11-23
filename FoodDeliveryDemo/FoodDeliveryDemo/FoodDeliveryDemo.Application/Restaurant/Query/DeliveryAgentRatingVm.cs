using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryDemo.Application.Restaurant.Query
{
	public class DeliveryAgentRatingVm
	{
		public Guid DeliveryAgentId { get; set; }

		public Guid UserId { get; set; }

		public int Rating { get; set; }

		public string Comment { get; set; }
	}
}
