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
		public async Task Get()
		{
			await _productWriteRepository.AddAsync(new()
			{
				Name = "Product 4",
				Stock = 10,
				Price = 15.2,
			});
			await _productWriteRepository.SaveAsync();
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			var data = await _productReadRepository.GetByIdAsync(id);

			data.Name = "Deyisdimi?";
			await _productWriteRepository.SaveAsync();
			return Ok(data);
		}
		
	}
}
