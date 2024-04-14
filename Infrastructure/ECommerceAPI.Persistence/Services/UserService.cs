using Azure.Core;
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services
{
	public class UserService : IUserService
	{
		readonly UserManager<AppUser> _userManager;

		public UserService(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}
		public async Task<CreateUserResponse> CreateAsync(CreateUser model)
		{
			IdentityResult result = await _userManager.CreateAsync(new AppUser()
			{
				Id = Guid.NewGuid().ToString(),
				UserName = model.UserName,
				Email = model.Email,
				FullName = model.FullName,
			}, model.Password);

			CreateUserResponse response = new() { Succeded = result.Succeeded };

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

		public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate,int addOnAccessToken)
		{
			if (user != null)
			{
				user.RefreshToken = refreshToken;
				user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessToken);
				await _userManager.UpdateAsync(user);
			}
			else
			   throw new Exception("User Not Found");
		}
	}
}
