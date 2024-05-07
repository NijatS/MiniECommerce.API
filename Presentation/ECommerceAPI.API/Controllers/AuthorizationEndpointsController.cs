using ECommerceAPI.Application.Features.Commands.AuthorizationEnpoints.AssignRole;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorizationEndpointsController : ControllerBase
	{
		private readonly IMediator _mediator;
		
		public AuthorizationEndpointsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> AssignRoleEnpoints(AssignRoleCommandRequest request)
		{
			request.Type = typeof(Program);
			AssignRoleCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
