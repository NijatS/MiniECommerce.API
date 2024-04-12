using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.Facebook;
using ECommerceAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUsers.FacebookLogin
{
	public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
	{
		readonly UserManager<AppUser> _userManager;
		readonly IConfiguration _configuration;
		readonly ITokenHandler _tokenHandler;
		readonly HttpClient _httpClient;

		public FacebookLoginCommandHandler(UserManager<AppUser> userManager, IConfiguration configuration, ITokenHandler tokenHandler, IHttpClientFactory httpClientFactory)
		{
			_userManager = userManager;
			_configuration = configuration;
			_tokenHandler = tokenHandler;
			_httpClient = httpClientFactory.CreateClient();
		}

		public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
		{

			string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");

			FacebookAccessToken? facebookAccessTokenDTO= JsonSerializer.Deserialize<FacebookAccessToken>( accessTokenResponse );

			string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={request.authToken}&access_token={facebookAccessTokenDTO?.AccessToken}");

			FacebookUserAccessValidation? validation = JsonSerializer.Deserialize<FacebookUserAccessValidation>(userAccessTokenValidation);

			if (validation.Data.isValid)
			{
				string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={request.authToken}");

				FacebookUserInfoResponse response =  JsonSerializer.Deserialize< FacebookUserInfoResponse>(userInfoResponse);



				UserLoginInfo info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");

				AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

				bool result = user != null;

				if (user == null)
				{
					user = await _userManager.FindByEmailAsync(response.Email);
					result = true;
					if (user == null)
					{
						user = new AppUser()
						{
							Id = Guid.NewGuid().ToString(),
							Email = response.Email,
							UserName = response.Email,
							FullName = response.Name
						};
						var identityResult = await _userManager.CreateAsync(user);
						result = identityResult.Succeeded;
					}

				}
		
					await _userManager.AddLoginAsync(user, info);
					Token token = _tokenHandler.CreateAccessToken(5);
					return new()
					{
						Token = token
					};

			}
			throw new Exception("Invalid external authentication");


		}
	}
}
