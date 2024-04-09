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
		readonly UserManager<AppUser> _userManager;
		readonly SignInManager<AppUser> _signInManager;

		public LoginCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
		{
			AppUser? user =  await _userManager.FindByNameAsync(request.UsernameOrEmail);

			if (user == null)
				user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);

			if (user == null)
				throw new NotFoundUserException("Username or password is incorrect");

		 SignInResult result = 	await _signInManager.CheckPasswordSignInAsync(user, request.Password,false);

			if (result.Succeeded)
			{

			}
			return new();
		}
	}
}
