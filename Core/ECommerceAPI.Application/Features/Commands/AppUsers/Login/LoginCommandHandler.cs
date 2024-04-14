using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUsers.Login
{
	public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
	{
		readonly IAuthService _service;

		public LoginCommandHandler(IAuthService service)
		{
			_service = service;
		}

		public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
		{
			var token = await _service.LoginAsync(request.UsernameOrEmail, request.Password, 15);
			return new LoginCommandSuccessResponse()
			{
				Token = token
			};
		}
	}
}
