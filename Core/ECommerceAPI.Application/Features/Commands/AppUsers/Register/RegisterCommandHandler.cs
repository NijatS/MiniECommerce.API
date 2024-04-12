using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUsers.Register
{
	public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse>
	{
		readonly IUserService _service;

		public RegisterCommandHandler(IUserService service)
		{
			_service = service;
		}

		public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
		{
		  CreateUserResponse response =	await _service.CreateAsync(new(){ 
				Email = request.Email,
				FullName = request.FullName,
				UserName = request.UserName,
				Password = request.Password,
				ConfirmPassword = request.ConfirmPassword,
			});

			return new()
			{
				Message = response.Message,
				Succeded = response.Succeded
			};
			
		}
	}
}
