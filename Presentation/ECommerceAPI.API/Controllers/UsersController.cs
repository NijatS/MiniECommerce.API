﻿using ECommerceAPI.Application.Features.Commands.AppUsers.FacebookLogin;
using ECommerceAPI.Application.Features.Commands.AppUsers.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.AppUsers.Login;
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
		[HttpPost("[action]")]
		public async Task<IActionResult> Login([FromBody]LoginCommandRequest loginCommandRequest)
		{
			LoginCommandResponse response = await _mediator.Send(loginCommandRequest);
			return Ok(response);
		}

		[HttpPost("google-login")]
		public async Task<IActionResult> GoogleLogin([FromBody]GoogleLoginCommandRequest googleLoginCommandRequest)
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
