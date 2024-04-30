using Azure.Core;
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
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
		readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;
		readonly ICompletedOrderReadRepository _completedOrderReadRepository;

		public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRepository, ICompletedOrderReadRepository completedOrderReadRepository)
		{
			_orderWriteRepository = orderWriteRepository;
			_orderReadRepository = orderReadRepository;
			_completedOrderWriteRepository = completedOrderWriteRepository;
			_completedOrderReadRepository = completedOrderReadRepository;
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


			var data2 = from order in data
				   join completedOrder in _completedOrderReadRepository.Table
				   on order.Id equals completedOrder.OrderId into co
				   from _co in co.DefaultIfEmpty()
				   select new
				   {
					   order.CreatedDate,
					   order.OrderCode,
					   order.Basket,
					   order.Id,
					   Completed = _co != null ? true : false,
				   };

			return new()
			{
				TotalCount = await query.CountAsync(),
				Orders = await data2.Select(o=> new {
					Id = o.Id,
					CreatedDate = o.CreatedDate,
					OrderCode = o.OrderCode,
					TotalPrice = (float)o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
					UserName = o.Basket.AppUser.UserName,
					Completed = o.Completed
				}).ToListAsync(),

			};

		
		}

		public async Task<SingleOrder> GetOrderByIdAsync(string id)
		{
			var data =  _orderReadRepository.Table
				.Include(o => o.Basket)
				 .ThenInclude(b => b.BasketItems)
				  .ThenInclude(bi => bi.Product);

			var data2 =await (from order in data
						 join completedOrder in _completedOrderReadRepository.Table
						 on order.Id equals completedOrder.OrderId into co
						 from _co in co.DefaultIfEmpty()
						 select new
						 {
							 order.CreatedDate,
							 order.OrderCode,
							 order.Basket,
							 order.Id,
							 order.Address,
							 order.Description,
							 Completed = _co != null ? true : false,
						 }).FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

			return new()
			{
				Id = data2.Id.ToString(),
				CreatedDate = data2.CreatedDate,
				OrderCode = data2.OrderCode,
				BasketItems = data2.Basket.BasketItems.Select(bi => new
				{
					bi.Product.Name,
					bi.Product.Price,
					bi.Quantity
				}),
				Address = data2.Address,
				Description = data2.Description,
				Completed = data2.Completed
			};
		}
		public async Task<(bool, CompletedOrderDto)> CompleteOrderAsync(string id)
		{
			Order? order = await _orderReadRepository.Table.Include(o => o.Basket)
				  .ThenInclude(b => b.AppUser)
					.FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

			if (order != null)
			{
				await _completedOrderWriteRepository.AddAsync(new()
				{
					OrderId = Guid.Parse(id),
				});
				return (await _completedOrderWriteRepository.SaveAsync()>0,new()
				{
					OrderCode = order.OrderCode,
					FullName = order.Basket.AppUser.FullName,
					OrderDate = order.CreatedDate,
					Email = order.Basket.AppUser.Email
				});
			}
			return (false,null);
		}

	}
}
