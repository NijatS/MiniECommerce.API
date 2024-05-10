using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Product.QRCode
{
	public class QRCodeQueryHandler : IRequestHandler<QRCodeQueryRequest, QRCodeQueryResponse>
	{
		readonly IProductService _productService;

		public QRCodeQueryHandler(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<QRCodeQueryResponse> Handle(QRCodeQueryRequest request, CancellationToken cancellationToken)
		{
			var datas = await _productService.QRCodeToProductAsync(request.ProductId);
			return new()
			{
				bytes = datas
			};

		}
	}
}
