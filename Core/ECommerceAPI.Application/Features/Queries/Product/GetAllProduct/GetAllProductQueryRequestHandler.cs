using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParametrs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryRequestHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
        readonly IProductService _productService;
		public GetAllProductQueryRequestHandler(IProductService productService)
		{
			_productService = productService;
		}
		public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _productService.GetAllProductsAsync(request.Page, request.Size);
            return new()
            {
                Products = data.Products,
                TotalCount = data.TotalCount,
            };
        }
    }
}
