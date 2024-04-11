using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUsers.GoogleLogin
{
	public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
	{
		readonly UserManager<AppUser> _userManager;
		readonly IConfiguration _configuration;
		readonly ITokenHandler _tokenHandler;

		public GoogleLoginCommandHandler(UserManager<AppUser> userManager, IConfiguration configuration, ITokenHandler tokenHandler)
		{
			_userManager = userManager;
			_configuration = configuration;
			_tokenHandler = tokenHandler;
		}

		public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
		{
			var settings = new GoogleJsonWebSignature.ValidationSettings()
			{
				Audience = new List<string> { "357752492573-ffekgnm8cnn2a7ab9biiok4beadoggjn.apps.googleusercontent.com" }
			};
			var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

			UserLoginInfo info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);

		    AppUser user = 	await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

			bool result = user != null;

			if (user == null)
			{
				user = await _userManager.FindByEmailAsync(payload.Email);
				if (user == null)
				{
					user = new AppUser()
					{
						Id = Guid.NewGuid().ToString(),
						Email = payload.Email,
						UserName = payload.Email,
						FullName = payload.Name
					};
				    var identityResult = await _userManager.CreateAsync(user);
					result = identityResult.Succeeded;
				}

			}
			if (result)
				await _userManager.AddLoginAsync(user, info);
		//	else
			//	throw new Exception("Invalid external authentication");

			Token token = _tokenHandler.CreateAccessToken(5);

			return new()
			{
				Token = token
			};

		}
	}
}
