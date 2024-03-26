using ECommerceAPI.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductWriteRepository _productWriteRepository;
		private readonly IProductReadRepository _productReadRepository;

		public ProductController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
		{
			_productWriteRepository = productWriteRepository;
			_productReadRepository = productReadRepository;
		}
		[HttpGet]
		public async void Get() {
			await _productWriteRepository.AddRangeAsync(new()
			{
				new()
				{
					Id=Guid.NewGuid(),
					CreatedDate= DateTime.Now,
					Name = "Product 1",
					Stock = 10,
					Price = 100,
				},
					new()
				{
					Id=Guid.NewGuid(),
					CreatedDate= DateTime.Now,
					Name = "Product 2",
					Stock = 10,
					Price = 100,
				},
						new()
				{
					Id=Guid.NewGuid(),
					CreatedDate= DateTime.Now,
					Name = "Product 3",
					Stock = 10,
					Price = 100,
				}
			});
			await _productWriteRepository.SaveAsync();
		}




	}
}
