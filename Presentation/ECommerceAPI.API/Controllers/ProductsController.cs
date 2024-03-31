using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParametrs;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerceAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductWriteRepository _productWriteRepository;
		private readonly IProductReadRepository _productReadRepository;

		public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
		{
			_productWriteRepository = productWriteRepository;
			_productReadRepository = productReadRepository;
		}
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] Pagination pagination)
		{
			var totalCount = _productReadRepository.GetAll(false).Count();
			var products = _productReadRepository.GetAll(false)
				.Select(
				p => new
				{
					p.Id,
					p.Name,
					p.Stock,
					p.Price,
					p.CreatedDate,
					p.UpdatedDate
				})
				.Skip(pagination.Page * pagination.Size)
				.Take(pagination.Size);
			return Ok(new
			{
				totalCount,
				products
			});
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			Product product = await _productReadRepository.GetByIdAsync(id,false);
			return Ok(product);
		}
		[HttpPost]
		public async Task<IActionResult> Post(VM_Product_Create model)
		{
			await _productWriteRepository.AddAsync(new(){
				Name = model.Name,
				Price = model.Price,
				Stock = model.Stock
			});
			await _productWriteRepository.SaveAsync();
			return StatusCode((int)HttpStatusCode.Created);
		}

		[HttpPut]
		public async Task<IActionResult> Put(VM_Product_Update model)
		{
			Product product = await _productReadRepository.GetByIdAsync(model.Id);

			product.Name = model.Name;
			product.Price = model.Price;
			product.Stock = model.Stock;

			await _productWriteRepository.SaveAsync();
			return StatusCode((int)HttpStatusCode.OK);

		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			await _productWriteRepository.Remove(id);
			await _productWriteRepository.SaveAsync();
			return Ok();
		}

		
	}
}
