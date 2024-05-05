﻿using Azure.Core;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Features.Commands.Role.CreateRole;
using ECommerceAPI.Application.Features.Commands.Role.DeleteRole;
using ECommerceAPI.Application.Features.Commands.Role.UpdateRole;
using ECommerceAPI.Application.Features.Queries.Role.GetRoleById;
using ECommerceAPI.Application.Features.Queries.Role.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Admin")]
	public class RolesController : ControllerBase
	{
		readonly IMediator _mediator;

		public RolesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{Id}")]
		[AuthorizeDefinition(ActionType =ActionType.Reading,Definition ="Get Role By Id",Menu ="Roles")]
		public async Task<IActionResult> GetRoles([FromRoute] GetRoleByIdQueryRequest request)
		{
			GetRoleByIdQueryResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet]
		[AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles", Menu = "Roles")]

		public async Task<IActionResult> GetRoles([FromQuery]GetRolesQueryRequest request)
		{
			GetRolesQueryResponse response = await _mediator.Send(request);
			return Ok(response);
		}
		[HttpPost]
		[AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Create Role", Menu = "Roles")]
		public async  Task<IActionResult> CreateRole([FromBody]CreateRoleCommandRequest request)
		{
			CreateRoleCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPut("{Id}")]
		[AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Update Role", Menu = "Roles")]
		public async Task<IActionResult> UpdateRole([FromBody,FromRoute]UpdateRoleCommandRequest request)
		{
			UpdateRoleCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}
		[HttpDelete("{Id}")]
		[AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Role", Menu = "Roles")]
		public async Task<ActionResult> DeleteRole([FromRoute]DeleteRoleCommandRequest request)
		{
			DeleteRoleCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
