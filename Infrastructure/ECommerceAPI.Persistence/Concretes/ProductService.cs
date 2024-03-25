using ECommerceAPI.Application.Abstraction;
using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Concretes
{
	public class ProductService : IProductService
	{
		public List<Product> GetProducts()
		{
			return new()
			{
				new()
				{
					Id= Guid.NewGuid(),
					CreatedDate = DateTime.Now,
					Name = "Product1",
					Price = 100,
					Stock = 10
				},
					new()
				{
					Id= Guid.NewGuid(),
					CreatedDate = DateTime.Now,
					Name = "Product2",
					Price = 100,
					Stock = 10
				},
						new()
				{
					Id= Guid.NewGuid(),
					CreatedDate = DateTime.Now,
					Name = "Product3",
					Price = 100,
					Stock = 10
				},
							new()
				{
					Id= Guid.NewGuid(),
					CreatedDate = DateTime.Now,
					Name = "Product4",
					Price = 100,
					Stock = 10
				}

			};
		}
	}
}

