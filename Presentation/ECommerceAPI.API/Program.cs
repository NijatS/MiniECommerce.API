using ECommerceAPI.Application.Validators.Products;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Infrastructure.Services.Storage.Azure;
using ECommerceAPI.Infrastructure.Services.Storage.Local;
using ECommerceAPI.Persistence;
using FluentValidation.AspNetCore;
namespace ECommerceAPI.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddPersistanceServices();
			builder.Services.AddInfrastructureServices();

			builder.Services.AddStorage<AzureStorage>();
			//builder.Services.AddStorage(StorageType.Local);

			builder.Services.AddControllers(option=> option.Filters.Add<ValidationFilter>())
				.AddFluentValidation(configuration=>configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
				.ConfigureApiBehaviorOptions(option=> option.SuppressModelStateInvalidFilter =true);
			builder.Services.AddCors(options=> options.AddDefaultPolicy(policy=> policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()));

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseStaticFiles();
			app.UseCors();

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
