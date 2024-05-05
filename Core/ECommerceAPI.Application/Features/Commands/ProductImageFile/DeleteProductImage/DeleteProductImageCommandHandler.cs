using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerceAPI.Application.Features.Commands.ProductImageFile.DeleteProductImage
{
	internal class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
	{

		readonly IProductService _productService;

		public DeleteProductImageCommandHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IProductService productService)
		{
		
			_productService = productService;
		}

		public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
		{

			await _productService.DeleteProductImage(request.Id, request.ImageId);
			return new();
		}
	}
}
