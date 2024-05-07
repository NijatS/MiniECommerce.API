using ECommerceAPI.Application.Abstractions.Services;
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
	
		readonly IProductService _productService;

		public UploadProductImageCommandHandler(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
		{
			await _productService.UploadProductImages(new DTOs.Product.UploadProductImages()
			{
				Files = request.Files,
				Id = request.Id,
			});
			return new();
		}
	}
}
