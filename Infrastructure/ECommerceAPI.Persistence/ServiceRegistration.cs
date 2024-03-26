
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Persistence.Contexts;
using ECommerceAPI.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
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
			service.AddDbContext<ECommerceAPIDbContext>(options => options.
		          UseSqlServer(Configuration.ConnectionString),ServiceLifetime.Singleton);
			service.AddSingleton<ICustomerReadRepository,CustomerReadRepository>();
			service.AddSingleton<ICustomerWriteRepository, CustomerWriteRepository>();
			service.AddSingleton<IOrderReadRepository, OrderReadRepository>();
			service.AddSingleton<IOrderWriteRepository, OrderWriteRepository>();
			service.AddSingleton<IProductReadRepository, ProductReadRepository>();
			service.AddSingleton<IProductWriteRepository, ProductWriteRepository>();
		}
	}
}
