using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Features.Commands.AppUsers.AssignRoleToUser;
using ECommerceAPI.Application.Features.Commands.AppUsers.Register;
using ECommerceAPI.Application.Features.Commands.AppUsers.UpdatePassword;
using ECommerceAPI.Application.Features.Commands.AuthorizationEnpoints.AssignRole;
using ECommerceAPI.Application.Features.Queries.AppUser.GetAllUsers;
using ECommerceAPI.Application.Features.Queries.AppUser.GetRolesToUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
		[HttpGet]
		[Authorize(AuthenticationSchemes ="Admin")]
		[AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get All Users", Menu = "Users")]
		public async Task<IActionResult> GetAllUsers([FromQuery]GetAllUsersQueryRequest request)
		{
			GetAllUsersQueryResponse response = await _mediator.Send(request);
			return Ok(response);
		}
		[HttpPost("assign-role-to-user")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Assign Role To User", Menu = "Users")]
		public async Task<IActionResult> AssignRoleToUser([FromBody]AssignRoleToUserCommandRequest request)
		{
			AssignRoleToUserCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("get-roles-to-user/{UserId}")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles To User", Menu = "Users")]
		public async Task<IActionResult> GetRolesToUser([FromRoute] GetRolesToUserQueryRequest request)
		{
			GetRolesToUserQueryResponse response = await _mediator.Send(request);
			return Ok(response);
		}

	}
}
