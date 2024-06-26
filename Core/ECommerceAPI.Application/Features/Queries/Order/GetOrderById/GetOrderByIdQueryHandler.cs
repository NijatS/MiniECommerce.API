﻿using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Order.GetOrderById
{
	public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
	{
		readonly IOrderService _orderService;

		public GetOrderByIdQueryHandler(IOrderService orderService)
		{
			_orderService = orderService;
		}

		public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
		{
			var data = await _orderService.GetOrderByIdAsync(request.Id);
			return new()
			{
				OrderCode = data.OrderCode,
				Description = data.Description,
				Id = data.Id,
				BasketItems = data.BasketItems,
				CreatedDate = data.CreatedDate,
				Address = data.Address,
				Completed = data.Completed
			};
		}
	}
}
