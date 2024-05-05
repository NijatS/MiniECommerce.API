using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.ProductImageFile.ChangeShowCaseImage
{
	public class ChangeShowCaseImageCommandHandler : IRequestHandler<ChangeShowCaseImageCommandRequest, ChangeShowCaseImageCommandResponse>
	{
		readonly IProductService _productService;

		public ChangeShowCaseImageCommandHandler( IProductService productService)
		{
			_productService = productService;
		}

		public async Task<ChangeShowCaseImageCommandResponse> Handle(ChangeShowCaseImageCommandRequest request, CancellationToken cancellationToken)
		{
			await _productService.ChangeShowCaseImage(request.ImageId, request.ProductId);
			return new();
		}
	}
}
