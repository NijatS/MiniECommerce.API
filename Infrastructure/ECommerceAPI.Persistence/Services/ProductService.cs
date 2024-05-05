using Azure.Core;
using ECommerceAPI.Application.Abstractions.Hubs;
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.DTOs.Product;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services
{
	public class ProductService : IProductService
	{
		readonly IProductWriteRepository _productWriteRepository;
		readonly IProductHubService _productHubService;
		readonly IProductReadRepository _productReadRepository;
		readonly IStorageService _storageService;
		readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
		public ProductService(IProductWriteRepository productWriteRepository, IProductHubService productHubService, IProductReadRepository productReadRepository, IStorageService storageService, IProductImageFileWriteRepository productImageFileWriteRepository)
		{
			_productWriteRepository = productWriteRepository;
			_productHubService = productHubService;
			_productReadRepository = productReadRepository;
			_storageService = storageService;
			_productImageFileWriteRepository = productImageFileWriteRepository;
		}

		public  async Task ChangeShowCaseImage(string imageId, string productId)
		{
			var query = _productImageFileWriteRepository.Table.Include(p => p.Products)
				.SelectMany(p => p.Products, (pif, p) => new
				{
					pif,
					p
				});
			var data = await query.FirstOrDefaultAsync(p => p.p.Id == Guid.Parse(productId) && p.pif.Showcase);

			if (data != null)
			{
				data.pif.Showcase = false;
			}

			var image = await query.FirstOrDefaultAsync(p => p.pif.Id == Guid.Parse(imageId));

			if (image != null)
			{
				image.pif.Showcase = true;
			}
			await _productImageFileWriteRepository.SaveAsync();
		}

		public async Task CreateProductAsync(CreateProduct createProduct)
		{
			await _productWriteRepository.AddAsync(new()
			{
				Name = createProduct.Name,
				Price = createProduct.Price,
				Stock = createProduct.Stock
			});
			await _productWriteRepository.SaveAsync();
			await _productHubService.ProductAddedMessageAsync($"{createProduct.Name} adli product elave olunmusdur");
		}

		public async Task DeleteProductImage(string id, string? imageId)
		{
			Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
		   .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

			ProductImageFile? imageFile = product?.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));

			if (imageFile != null)
			{
				product?.ProductImageFiles.Remove(imageFile);
			}
			await _productWriteRepository.SaveAsync();
		}

		public  async Task<ListProduct> GetAllProductsAsync(int page, int size)
		{
			var totalProductCount = _productReadRepository.GetAll(false).Count();
			var products = _productReadRepository.GetAll(false)
				.Include(p => p.ProductImageFiles)
				.Select(
				p => new
				{
					p.Id,
					p.Name,
					p.Stock,
					p.Price,
					p.CreatedDate,
					p.UpdatedDate,
					p.ProductImageFiles
				})
				.Skip(page *size)
				.Take(size);
			return new ListProduct()
			{
				Products = products,
				TotalCount = totalProductCount
			};
		}

		public async  Task<SingleProduct> GetProductByIdAsync(string id)
		{
			Product product = await _productReadRepository.GetByIdAsync(id, false);
			return new SingleProduct()
			{
				Name = product.Name,
				Price = product.Price,
				Stock = product.Stock,
			};
		}

		public  async Task RemoveProductAsync(string id)
		{
			await _productWriteRepository.Remove(id);
			await _productWriteRepository.SaveAsync();
		}

		public async Task UpdateProductAsync(UpdateProduct updateProduct)
		{
		    Product product = await _productReadRepository.GetByIdAsync(updateProduct.Id);

			product.Name = updateProduct.Name;
			product.Price = updateProduct.Price;
			product.Stock = updateProduct.Stock;

			_productWriteRepository.Update(product);
			await _productWriteRepository.SaveAsync();
		}

		public async Task UploadProductImages(UploadProductImages uploadProductImages)
		{
			List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("resource/products", uploadProductImages.Files);

		   Product product = await _productReadRepository.GetByIdAsync(uploadProductImages.Id);

			await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ECommerceAPI.Domain.Entities.ProductImageFile
			{
				FileName = r.fileName,
				Path = r.pathOrContainerName,
				Storage = _storageService.StorageName,
				Products = new List<Product>() { product }
			}).ToList());

			await _productImageFileWriteRepository.SaveAsync();
		}
	}
}
