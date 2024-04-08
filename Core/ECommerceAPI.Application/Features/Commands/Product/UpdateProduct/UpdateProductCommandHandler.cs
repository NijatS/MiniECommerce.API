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
		public UpdateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
		{
			_productWriteRepository = productWriteRepository;
			_productReadRepository = productReadRepository;
		}

		public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
		{
			ECommerceAPI.Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);

			product.Name = request.Name;
			product.Price = request.Price;
			product.Stock = request.Stock;

			await _productWriteRepository.SaveAsync();
			return new();
		}
	}
}
