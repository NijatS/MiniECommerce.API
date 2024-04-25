using Azure.Core;
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services
{
	public class OrderService : IOrderService
	{
		readonly IOrderWriteRepository _orderWriteRepository;
		readonly IOrderReadRepository _orderReadRepository;

		public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository)
		{
			_orderWriteRepository = orderWriteRepository;
			_orderReadRepository = orderReadRepository;
		}

		public async Task CreateOrderAsync(CreateOrder createOrder)
		{
			string orderCode = (new Random().NextDouble() * 10000).ToString();
			orderCode = orderCode.Substring(orderCode.IndexOf(".") + 1, orderCode.Length - orderCode.IndexOf(".") - 1);
			await _orderWriteRepository.AddAsync(new()
			{
				Address = createOrder.Address,
				Description = createOrder.Description,
				Id = Guid.Parse(createOrder.BasketId),
				OrderCode = orderCode
			});
			await _orderWriteRepository.SaveAsync();
		}

		public async Task<ListOrder> GetAllOrdersAsync(int page, int size)
		{
			var query = _orderReadRepository.Table.Include(o => o.Basket)
				 .ThenInclude(b => b.AppUser)
				.Include(o => o.Basket)
				 .ThenInclude(b => b.BasketItems)
				  .ThenInclude(bi => bi.Product);

			var data = query.Skip(page * size)
				.Take(size);

			return new()
			{
				TotalCount = await query.CountAsync(),
				Orders = await data.Select(o=> new {
					CreatedDate = o.CreatedDate,
					OrderCode = o.OrderCode,
					TotalPrice = (float)o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
					UserName = o.Basket.AppUser.UserName
				}).ToListAsync(),

			};

		
		}
	}
}
