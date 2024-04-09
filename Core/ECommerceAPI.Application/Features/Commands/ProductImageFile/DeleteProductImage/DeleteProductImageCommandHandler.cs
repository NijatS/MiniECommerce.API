﻿using ECommerceAPI.Application.Repositories;
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
		readonly IProductReadRepository _productReadRepository;
		readonly IProductWriteRepository _productWriteRepository;

		public DeleteProductImageCommandHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
		{
			_productWriteRepository = productWriteRepository;
			_productReadRepository = productReadRepository;
		}

		public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
		{
		ECommerceAPI.Domain.Entities.Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
			.FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

			ECommerceAPI.Domain.Entities.ProductImageFile? imageFile = product?.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(request.ImageId));

			if (imageFile != null)
			{
				product?.ProductImageFiles.Remove(imageFile);
			}
			await _productWriteRepository.SaveAsync();
			return new();
		}
	}
}