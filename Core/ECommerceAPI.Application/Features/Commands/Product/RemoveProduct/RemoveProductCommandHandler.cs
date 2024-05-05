using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Product.RemoveProduct
{
	public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommandRequest, RemoveProductCommandResponse>
	{
		readonly IProductService _productService;

		public RemoveProductCommandHandler( IProductService productService)
		{
			_productService = productService;
		}

		public async Task<RemoveProductCommandResponse> Handle(RemoveProductCommandRequest request, CancellationToken cancellationToken)
		{
			await _productService.RemoveProductAsync(request.Id);
			return new();
		}
	}
}
