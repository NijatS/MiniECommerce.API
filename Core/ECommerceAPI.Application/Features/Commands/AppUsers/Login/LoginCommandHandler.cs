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
		readonly UserManager<AppUser> _userManager;
		readonly SignInManager<AppUser> _signInManager;
		readonly ITokenHandler _tokenHandler;

		public LoginCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenHandler = tokenHandler;
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
			   Token token =_tokenHandler.CreateAccessToken(5);
				return new LoginCommandSuccessResponse() { Token=token };
			}
			return new LoginCommandErrorResponse() { Message = "Username or password is incorrect" };
		}
	}
}
