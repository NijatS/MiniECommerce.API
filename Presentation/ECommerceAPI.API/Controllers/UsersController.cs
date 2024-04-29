using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Features.Commands.AppUsers.Register;
using ECommerceAPI.Application.Features.Commands.AppUsers.UpdatePassword;
using MediatR;
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
		public async Task<IActionResult> Register([FromBody] RegisterCommandRequest request)
		{
		RegisterCommandResponse response = 	await _mediator.Send(request);
			return Ok(response);
		}
		[HttpPost("update-password")]
		public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest request)
		{
			UpdatePasswordCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}

	}
}
