using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AuthorizationEnpoints.AssignRole
{
	public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommandRequest, AssignRoleCommandResponse>
	{
		readonly IAuthorizationEndpointService _service;

		public AssignRoleCommandHandler(IAuthorizationEndpointService service)
		{
			_service = service;
		}

		public async Task<AssignRoleCommandResponse> Handle(AssignRoleCommandRequest request, CancellationToken cancellationToken)
		{
			await _service.AssignRoleEndpointAsync(request.Roles, request.Code, request.Menu, request.Type);
			return new()
			{

			};
		}
	}
}
