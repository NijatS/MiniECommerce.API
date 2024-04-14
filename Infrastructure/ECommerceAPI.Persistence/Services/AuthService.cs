using Azure.Core;
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.Facebook;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Application.Features.Commands.AppUsers.Login;
using ECommerceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services
{
	public class AuthService : IAuthService
	{
		readonly HttpClient _httpClient;
		readonly IConfiguration _configuration;
		readonly UserManager<AppUser> _userManager;
		readonly ITokenHandler _tokenHandler;
		readonly SignInManager<AppUser> _signInManager;
		readonly IUserService _userService;
		public AuthService(IHttpClientFactory httpClient, IConfiguration configuration, UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService)
		{
			_httpClient = httpClient.CreateClient();
			_configuration = configuration;
			_userManager = userManager;
			_tokenHandler = tokenHandler;
			_signInManager = signInManager;
			_userService = userService;
		}
		public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
		{
			string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");

			FacebookAccessToken? facebookAccessTokenDTO = JsonSerializer.Deserialize<FacebookAccessToken>(accessTokenResponse);

			string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenDTO?.AccessToken}");

			FacebookUserAccessValidation? validation = JsonSerializer.Deserialize<FacebookUserAccessValidation>(userAccessTokenValidation);


			if (validation?.Data.isValid != null)
			{
				string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");

				FacebookUserInfoResponse? response = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

				UserLoginInfo info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");

				AppUser? user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

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
				Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);

				await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 5);
				return token;
			}
			throw new Exception("Invalid external authentication");
		}

		public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
		{
			var settings = new GoogleJsonWebSignature.ValidationSettings()
			{
				Audience = new List<string> { _configuration["ExternalLoginSettings:Google:Client_ID"] }
			};
			var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

			UserLoginInfo info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");

			AppUser? user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

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
			else
				throw new Exception("Invalid external authentication");

			Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
			await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 5);

			return token;

		}

		public async Task<Token> LoginAsync(string userNameOrEmail, string password, int accessTokenLifeTime)
		{
			AppUser? user = await _userManager.FindByNameAsync(userNameOrEmail);

			if (user == null)
				user = await _userManager.FindByEmailAsync(userNameOrEmail);

			if (user == null)
				throw new NotFoundUserException("Username or password is incorrect");

			SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

			if (result.Succeeded)
			{
				Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);

				await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 5);

				return token;
			}
			throw new UnauthorizedAccessException("User Not found");
		}

		public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
		{
			AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
			if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
			{
				Token token = _tokenHandler.CreateAccessToken(15);
				await _userService.UpdateRefreshToken(token.RefreshToken,user,token.Expiration,15);
				return token;
			}
			throw new NotFoundUserException();
		}
	}
}
