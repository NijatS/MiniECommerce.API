using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.AuthorizationEndpoints.GetRolestoEndpoints
{
	public class GetRolestoEndpointQueryHandler : IRequestHandler<GetRolestoEndpointQueryRequest, GetRolestoEndpointQueryResponse>
	{
		readonly IAuthorizationEndpointService _authorizationEndpointService;

		public GetRolestoEndpointQueryHandler(IAuthorizationEndpointService authorizationEndpointService)
		{
			_authorizationEndpointService = authorizationEndpointService;
		}

		public async Task<GetRolestoEndpointQueryResponse> Handle(GetRolestoEndpointQueryRequest request, CancellationToken cancellationToken)
		{
			var roles = await _authorizationEndpointService.GetRolesToEndpointAsync(request.Code,request.Menu);
			return new()
			{
				Roles = roles
			};
		}
	}
}
