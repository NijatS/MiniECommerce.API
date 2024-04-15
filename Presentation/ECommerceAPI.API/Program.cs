using ECommerceAPI.API.Configurations.ColumnWriters;
using ECommerceAPI.Application;
using ECommerceAPI.Application.Validators.Products;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Infrastructure.Services.Storage.Azure;
using ECommerceAPI.Infrastructure.Services.Storage.Local;
using ECommerceAPI.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Common;
using System.Security.Claims;
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

			builder.Services.AddControllers(option => option.Filters.Add<ValidationFilter>())
				.AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
				.ConfigureApiBehaviorOptions(option => option.SuppressModelStateInvalidFilter = true);
			builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()));


			SqlColumn sqlColumn = new SqlColumn();
			sqlColumn.ColumnName = "Username";
			sqlColumn.DataType = System.Data.SqlDbType.NVarChar;
			sqlColumn.PropertyName = "Username";
			sqlColumn.DataLength = 50;
			sqlColumn.AllowNull = true;
			ColumnOptions columnOpt = new ColumnOptions();
			columnOpt.Store.Remove(StandardColumn.Properties);
			columnOpt.Store.Add(StandardColumn.LogEvent);
			columnOpt.AdditionalColumns = new Collection<SqlColumn> { sqlColumn };

			Logger log = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.File("logs/log.txt")
	.WriteTo.MSSqlServer(
	connectionString: builder.Configuration.GetConnectionString("SqlServer"),
	 sinkOptions: new MSSqlServerSinkOptions
	 {
		 AutoCreateSqlTable = true,
		 TableName = "Logs",
	 },
	 appConfiguration: null,
	 columnOptions: columnOpt
	)
	.Enrich.FromLogContext()
	.Enrich.With<CustomUserNameColumn>()
	.MinimumLevel.Information()
	.CreateLogger();




			builder.Host.UseSerilog(log);

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer("Admin", options =>
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
						LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
						NameClaimType = ClaimTypes.Name
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
			app.UseSerilogRequestLogging();

			app.UseCors();

			app.UseHttpsRedirection();


			app.UseAuthentication();
			app.UseAuthorization();

			app.Use(async (context, next) =>
			{
				var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
				LogContext.PushProperty("Username", username);
				await next();
			});


			app.MapControllers();

			app.Run();
		}
	}
}
