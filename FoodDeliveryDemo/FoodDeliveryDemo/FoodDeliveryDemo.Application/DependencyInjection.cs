using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FoodDeliveryDemo.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
			return services;
		}
	}
}