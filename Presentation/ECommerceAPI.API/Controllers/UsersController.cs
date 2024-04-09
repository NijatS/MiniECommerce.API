using ECommerceAPI.Application.Features.Commands.AppUsers.Register;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		readonly IMediator _mediator;

		public UsersController(IMediator mediator)
		{
			_mediator = mediator;
		}
		[HttpPost]
		public async Task<IActionResult> Register([FromBody] RegisterCommandRequest registerCommandRequest)
		{
		RegisterCommandResponse response = 	await _mediator.Send(registerCommandRequest);
			return Ok(response);
		}
	}
}
