
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.Files;
using ECommerceAPI.Persistence.Contexts;
using ECommerceAPI.Persistence.Repositories;
using ECommerceAPI.Persistence.Repositories.File;
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
		          UseSqlServer(Configuration.ConnectionString));

			service.AddScoped<ICustomerReadRepository,CustomerReadRepository>();
			service.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();

			service.AddScoped<IOrderReadRepository, OrderReadRepository>();
			service.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

			service.AddScoped<IProductReadRepository, ProductReadRepository>();
			service.AddScoped<IProductWriteRepository, ProductWriteRepository>();

			service.AddScoped<IFileReadRepository, FileReadRepository>();
			service.AddScoped<IFileWriteRepository, FileWriteRepository>();

			service.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
			service.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();


			service.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
			service.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();
		}
	}
}
