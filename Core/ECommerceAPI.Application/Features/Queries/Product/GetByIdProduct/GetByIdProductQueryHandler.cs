using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Product.GetByIdProduct
{
	public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
	{
		readonly IProductService _productService;

		public GetByIdProductQueryHandler(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
		{
			var data = await _productService.GetProductByIdAsync(request.Id);
			return new()
			{
				Name = data.Name,
				Price = data.Price,
				Stock = data.Stock,
			};
		}
	}
}
