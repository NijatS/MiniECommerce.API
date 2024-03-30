using ECommerceAPI.Persistence;
namespace ECommerceAPI.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddPersistanceServices();
			builder.Services.AddControllers();
			builder.Services.AddCors(options=> options.AddDefaultPolicy(policy=> policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()));
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseCors();

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
