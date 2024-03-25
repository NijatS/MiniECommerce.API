using ECommerceAPI.Application.Abstraction;
using ECommerceAPI.Persistence.Concretes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence
{
	public static class ServiceRegistration
	{
		public static void AddPersistanceServices(this IServiceCollection service)
		{
			service.AddSingleton<IProductService,ProductService>();
		}
	}
}
