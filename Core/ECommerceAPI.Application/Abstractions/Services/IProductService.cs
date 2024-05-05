using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerceAPI.Application.Abstractions.Services
{
	public interface IProductService
	{
		Task CreateProductAsync(CreateProduct createProduct);
		Task UpdateProductAsync(UpdateProduct updateProduct);
		Task<SingleProduct> GetProductByIdAsync(string id);
		Task<ListProduct> GetAllProductsAsync(int page, int size);	
		Task RemoveProductAsync(string id);
		Task UploadProductImages(UploadProductImages uploadProductImages);
		Task ChangeShowCaseImage(string imageId, string productId);
		Task DeleteProductImage(string id,string? imageId);

	}
}
