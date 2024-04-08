using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Features.Commands.CreateProduct;
using ECommerceAPI.Application.Features.Queries.GetAllProduct;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParametrs;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Infrastructure.Services;
using MediatR;
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
		 readonly IProductWriteRepository _productWriteRepository;
		 readonly IProductReadRepository _productReadRepository;
		 readonly IWebHostEnvironment _webHostEnvironment;
		 readonly IStorageService _storageService;
		 readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
		readonly IConfiguration _configuration;

		readonly IMediator _mediator;
		public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnvironment, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService, IConfiguration configuration, IMediator mediator)
		{
			_productWriteRepository = productWriteRepository;
			_productReadRepository = productReadRepository;
			_webHostEnvironment = webHostEnvironment;
			_productImageFileWriteRepository = productImageFileWriteRepository;
			_storageService = storageService;
			_configuration = configuration;
			_mediator = mediator;
		}
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
		{
		  GetAllProductQueryResponse response =	await _mediator.Send(getAllProductQueryRequest);
		  return Ok(response);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			Product product = await _productReadRepository.GetByIdAsync(id,false);
			return Ok(product);
		}
		[HttpPost]
		public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
		{
		    CreateProductCommandResponse response =	await _mediator.Send(createProductCommandRequest);
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
		[HttpPost("[action]")]
		public async Task<IActionResult> Upload(string id)
		{
			List<(string fileName,string pathOrContainerName)> result =  await _storageService.UploadAsync("resource/products", Request.Form.Files);

			Product product = await _productReadRepository.GetByIdAsync(id);

			await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ProductImageFile { 
				FileName = r.fileName, 
				Path = r.pathOrContainerName , 
				Storage = _storageService.StorageName,
				Products = new List<Product>() { product}
			}).ToList());

			await _productImageFileWriteRepository.SaveAsync();

			return Ok();
		}
		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> GetProductImages(string id)
		{
			Product? product =await _productReadRepository.Table.Include(p=> p.ProductImageFiles)
				.FirstOrDefaultAsync(p=>p.Id == Guid.Parse(id));

			return Ok(product.ProductImageFiles.Select(p => new
			{
				Path = Path.Combine(_configuration["BaseStorageUrl"],p.Path),
				p.FileName,
				p.Id
			}));
		}

		[HttpDelete("[action]/{id}")]
		public async Task<IActionResult> DeleteProductImage(string id,string imageId)
		{
			Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
				.FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

			ProductImageFile? imageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));

			product.ProductImageFiles.Remove(imageFile);
			await _productWriteRepository.SaveAsync();
			return Ok();
		}
	}
}
