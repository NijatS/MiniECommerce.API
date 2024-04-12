using ECommerceAPI.Application.Features.Commands.AppUsers.FacebookLogin;
using ECommerceAPI.Application.Features.Commands.AppUsers.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.AppUsers.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{

		readonly IMediator _mediator;

		public AuthController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Login([FromBody] LoginCommandRequest loginCommandRequest)
		{
			LoginCommandResponse response = await _mediator.Send(loginCommandRequest);
			return Ok(response);
		}

		[HttpPost("google-login")]
		public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginCommandRequest googleLoginCommandRequest)
		{
			GoogleLoginCommandResponse response = await _mediator.Send(googleLoginCommandRequest);
			return Ok(response);
		}
		[HttpPost("facebook-login")]
		public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginCommandRequest facebookLoginCommandRequest)
		{
			FacebookLoginCommandResponse response = await _mediator.Send(facebookLoginCommandRequest);
			return Ok(response);
		}

	}
}
