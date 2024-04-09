using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUsers.Register
{
	public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse>
	{
		readonly UserManager<AppUser> _userManager;

		public RegisterCommandHandler(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
		{
		IdentityResult result =	await _userManager.CreateAsync(new AppUser()
			{
			   Id = Guid.NewGuid().ToString(),
				UserName = request.UserName,
				Email = request.Email,
				FullName = request.FullName,
			},request.Password);


			RegisterCommandResponse response = new() { Succeded = result.Succeeded };

			if (result.Succeeded)
				response.Message = "User Successfully Registered ";
			else
			{
				foreach (var error in result.Errors)
				{
					response.Message += $"{error.Code} - {error.Description}\n";

				}
			}
			return response;
			
		}
	}
}
