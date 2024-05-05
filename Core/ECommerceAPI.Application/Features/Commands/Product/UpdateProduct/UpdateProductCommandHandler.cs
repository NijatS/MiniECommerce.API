using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Product.UpdateProduct
{
	public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
	{
		readonly IProductWriteRepository _productWriteRepository;
		readonly IProductReadRepository _productReadRepository;
		readonly IProductService _productService;
		public UpdateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IProductService productService)
		{
			_productWriteRepository = productWriteRepository;
			_productReadRepository = productReadRepository;
			_productService = productService;
		}

		public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
		{
			await _productService.UpdateProductAsync(new()
			{
				Id = request.Id,
				Name = request.Name,
				Price = request.Price,
				Stock = request.Stock,
			});
		
			return new();
		}
	}
}
