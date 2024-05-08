using ECommerceAPI.Application.Features.Commands.AuthorizationEnpoints.AssignRole;
using ECommerceAPI.Application.Features.Queries.AuthorizationEndpoints.GetRolestoEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Admin")]

	public class AuthorizationEndpointsController : ControllerBase
	{
		private readonly IMediator _mediator;
		
		public AuthorizationEndpointsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> AssignRoleEnpoints([FromBody]AssignRoleCommandRequest request)
		{
			request.Type = typeof(Program);
			AssignRoleCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("get-roles-to-endpoint")]
		public async Task<IActionResult> GetRolestoEndpoints([FromBody]GetRolestoEndpointQueryRequest request)
		{
			GetRolestoEndpointQueryResponse response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
