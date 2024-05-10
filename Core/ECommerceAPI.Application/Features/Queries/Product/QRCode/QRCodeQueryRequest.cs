using MediatR;

namespace ECommerceAPI.Application.Features.Queries.Product.QRCode
{
	public class QRCodeQueryRequest:IRequest<QRCodeQueryResponse>
	{
		public string ProductId { get; set; }
	}
}