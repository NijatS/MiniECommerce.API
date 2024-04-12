using ECommerceAPI.Application;
using ECommerceAPI.Application.Validators.Products;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Infrastructure.Services.Storage.Azure;
using ECommerceAPI.Infrastructure.Services.Storage.Local;
using ECommerceAPI.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace ECommerceAPI.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddPersistanceServices();
			builder.Services.AddInfrastructureServices();
			builder.Services.AddApplicationService();

			builder.Services.AddStorage<LocalStorage>();
			//builder.Services.AddStorage(StorageType.Local);

			builder.Services.AddControllers(option=> option.Filters.Add<ValidationFilter>())
				.AddFluentValidation(configuration=>configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
				.ConfigureApiBehaviorOptions(option=> option.SuppressModelStateInvalidFilter =true);
			builder.Services.AddCors(options=> options.AddDefaultPolicy(policy=> policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()));


			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer("Admin",options =>
				{
					options.TokenValidationParameters = new()
					{
						ValidateAudience = true,
						ValidateIssuer = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidAudience = builder.Configuration["Token:Audience"],
						ValidIssuer = builder.Configuration["Token:Issuer"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),

					};
				});

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


			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
