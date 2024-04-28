using ECommerceAPI.Application.Features.Commands.AppUsers.FacebookLogin;
using ECommerceAPI.Application.Features.Commands.AppUsers.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.AppUsers.Login;
using ECommerceAPI.Application.Features.Commands.AppUsers.PasswordReset;
using ECommerceAPI.Application.Features.Commands.AppUsers.RefreshTokenLogin;
using ECommerceAPI.Application.Features.Commands.AppUsers.VerifyResetToken;
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
		public async Task<IActionResult> Login([FromBody] LoginCommandRequest request)
		{
			LoginCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenLoginCommandRequest request)
		{
			RefreshTokenLoginCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("google-login")]
		public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginCommandRequest request)
		{
			GoogleLoginCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}
		[HttpPost("facebook-login")]
		public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginCommandRequest request)
		{
			FacebookLoginCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}
		[HttpPost("password-reset")]
		public async Task<IActionResult> PasswordReset([FromBody] PasswordResetCommandRequest request)
		{
			PasswordResetCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}
		[HttpPost("verify-reset-token")]
		public async Task<IActionResult> VerifyResetToken([FromBody]VerifyResetTokenCommandRequest  request)
		{
			VerifyResetTokenCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}

	}
}
