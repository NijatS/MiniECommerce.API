using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Features.Commands.AppUsers.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		readonly IMediator _mediator;
		readonly IMailService _mailService;

		public UsersController(IMediator mediator, IMailService mailService)
		{
			_mediator = mediator;
			_mailService = mailService;
		}
		[HttpPost]
		public async Task<IActionResult> Register([FromBody] RegisterCommandRequest registerCommandRequest)
		{
		RegisterCommandResponse response = 	await _mediator.Send(registerCommandRequest);
			return Ok(response);
		}
		[HttpGet]
		public async Task<IActionResult> ExampleMail()
		{
			await _mailService.SendMessageAsync("nijatsoltanli03@gmail.com", "Test Mail","salam salam", false);
			return Ok();
		}
	}
}
