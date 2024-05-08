using MediatR;

namespace ECommerceAPI.Application.Features.Queries.AuthorizationEndpoints.GetRolestoEndpoints
{
	public class GetRolestoEndpointQueryRequest :IRequest<GetRolestoEndpointQueryResponse>
	{
		public string Code { get; set; }
		public string Menu { get; set; }
	}
}