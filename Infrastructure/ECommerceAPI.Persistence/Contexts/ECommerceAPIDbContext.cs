using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Common;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Contexts
{
	public class ECommerceAPIDbContext : IdentityDbContext<AppUser,AppRole,string>
	{
		public ECommerceAPIDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Domain.Entities.File> Files { get; set; }
		public DbSet<ProductImageFile> ProductImageFiles { get; set; }
		public DbSet<InvoiceFile> InvoiceFiles { get; set; }
		public DbSet<Basket> Basket { get; set; }
		public DbSet<BasketItem> BasketItems { get; set; }
		public DbSet<CompletedOrder> CompletedOrders { get; set; }
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var datas = ChangeTracker.Entries<BaseEntity>();

			foreach (var data in datas)
			{
				if(data.State == EntityState.Added)
				{
				data.Entity.CreatedDate = DateTime.Now;
				}
				else if(data.State == EntityState.Modified)
				{
					data.Entity.UpdatedDate = DateTime.Now;
				}
			}
			return base.SaveChangesAsync(cancellationToken);
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Order>()
				.HasKey(b => b.Id);

			builder.Entity<Order>()
				.HasIndex(o => o.OrderCode).IsUnique();

			builder.Entity<Basket>()
				.HasOne(b => b.Order)
				.WithOne(o => o.Basket)
				.HasForeignKey<Order>(o => o.Id);


			builder.Entity<Order>()
				.HasOne(o => o.CompletedOrder)
				 .WithOne(c=> c.Order)
				 .HasForeignKey<CompletedOrder>(c => c.OrderId);


			base.OnModelCreating(builder);

		}
	}
}
