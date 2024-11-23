using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryDemo.Application.Restaurant.Query
{
	public class OrdersVm
	{
		public Guid OrderId { get; set; }

		public Guid UserId { get; set; }

		public string UserName { get; set; }

		public Guid RestaurantId { get; set; }

		public string RestaurantName { get; set; }

		public Guid DeliveryAgentId { get; set; }

		public string DeliveryAgentName { get; set; }

		public Guid OrderStatusId { get; set; }

		public string OrderStatus { get; set; }

		public decimal TotalAmount { get; set; }

		public DateTime OrderDate {  get; set; }

		public List<OrderDetail> OrderDetails { get; set; }

		public class OrderDetail
		{
			public Guid OrderDetailsId { get; set;}
			public Guid OrderId { get; set; }
			public Guid MenuItemId { get; set; }
			public string MenuItemName { get; set; }
			public int Quantity { get; set; }
			public decimal Amount { get; set; }


		}

	}
}
