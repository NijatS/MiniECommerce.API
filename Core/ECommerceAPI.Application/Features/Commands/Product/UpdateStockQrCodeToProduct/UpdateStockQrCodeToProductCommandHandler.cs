using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Product.UpdateStockQrCodeToProduct
{
	public class UpdateStockQrCodeToProductCommandHandler : IRequestHandler<UpdateStockQrCodeToProductCommandRequest, UpdateStockQrCodeToProductCommandResponse>
	{
		readonly IProductService _productService;

		public UpdateStockQrCodeToProductCommandHandler(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<UpdateStockQrCodeToProductCommandResponse> Handle(UpdateStockQrCodeToProductCommandRequest request, CancellationToken cancellationToken)
		{
		   SingleProduct product = await _productService.GetProductByIdAsync(request.ProductId);

			await _productService.UpdateProductAsync(new()
			{
				Id = request.ProductId,
				Name = product.Name,
				Price = (float)product.Price,
				Stock = request.Stock,
			});
			return new() { };
		}
	}
}
