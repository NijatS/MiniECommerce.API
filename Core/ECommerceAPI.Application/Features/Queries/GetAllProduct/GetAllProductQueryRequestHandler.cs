using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParametrs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.GetAllProduct
{
	public class GetAllProductQueryRequestHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
	{
       readonly IProductReadRepository _productReadRepository;
		public GetAllProductQueryRequestHandler(IProductReadRepository productReadRepository)
		{
			_productReadRepository = productReadRepository;
		}
		public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
		{
			var totalCount = _productReadRepository.GetAll(false).Count();
			var products = _productReadRepository.GetAll(false)
				.Select(
				p => new
				{
					p.Id,
					p.Name,
					p.Stock,
					p.Price,
					p.CreatedDate,
					p.UpdatedDate
				})
				.Skip(request.Page * request.Size)
				.Take(request.Size);
			return new()
			{
				Products = products,
				TotalCount = totalCount
			};
		}
	}
}
