using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
	public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
	{
		readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
		readonly IStorageService _storageService;
		readonly IProductReadRepository _productReadRepository;

		public UploadProductImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService, IProductReadRepository productReadRepository)
		{
			_productImageFileWriteRepository = productImageFileWriteRepository;
			_storageService = storageService;
			_productReadRepository = productReadRepository;
		}

		public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
		{
			List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("resource/products", request.Files);

		   ECommerceAPI.Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);

			await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ECommerceAPI.Domain.Entities.ProductImageFile
			{
				FileName = r.fileName,
				Path = r.pathOrContainerName,
				Storage = _storageService.StorageName,
				Products = new List<ECommerceAPI.Domain.Entities.Product>() { product }
			}).ToList());

			await _productImageFileWriteRepository.SaveAsync();
			return new();
		}
	}
}
