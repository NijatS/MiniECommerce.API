
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Services.Authentications;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.Files;
using ECommerceAPI.Domain.Entities.Identity;
using ECommerceAPI.Persistence.Contexts;
using ECommerceAPI.Persistence.Repositories;
using ECommerceAPI.Persistence.Repositories.File;
using ECommerceAPI.Persistence.Services;
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

			service.AddIdentity<AppUser, AppRole>(options =>
			{
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireDigit = true;
				options.User.RequireUniqueEmail = true;
			}
				
				).AddEntityFrameworkStores<ECommerceAPIDbContext>();

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

			service.AddScoped<IBasketReadRepository, BasketReadRepository>();
			service.AddScoped<IBasketWriteRepository, BasketWriteRepository>();

			service.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
			service.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();

			service.AddScoped<IUserService,UserService>();
			service.AddScoped<IAuthService, AuthService>();

			service.AddScoped<IExternalAuthentication, AuthService>();
			service.AddScoped<IInternalAuthentication, AuthService>();
			service.AddScoped<IBasketService, BasketService>();

		}
	}
}
