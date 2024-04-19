namespace ECommerceAPI.Application.Features.Queries.Basket.GetBasketItems
{
	public class GetBasketItemsQueryResponse
	{
		public string Name { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }
		public string BasketItemId { get; set; }
	}
}