using Azure.Core;
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Helpers;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
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
		readonly IEndpointReadRepository _endpointReadRepository;


		public UserService(UserManager<AppUser> userManager, IEndpointReadRepository endpointReadRepository)
		{
			_userManager = userManager;
			_endpointReadRepository = endpointReadRepository;
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
		public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessToken)
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
		public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
		{
			AppUser? user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				resetToken = resetToken.UrlDecode();
				IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
				if (result.Succeeded)
					await _userManager.UpdateSecurityStampAsync(user);
				else
					throw new Exception("Password is changed failed");

			}
		}

		public async Task<List<ListUser>> GetAllUsersAsync(int page, int size)
		{
			var users = await _userManager.Users
				.Skip(page * size)
				.Take(size)
				.ToListAsync();

			return users.Select(user => new ListUser
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName,
				FullName = user.FullName,
				TwoFactor = user.TwoFactorEnabled

			}).ToList();
		}

		public async Task AssignRoleToUserAsync(string userId, string[] roles)
		{
			AppUser? user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				var userRoles = await _userManager.GetRolesAsync(user);
				await _userManager.RemoveFromRolesAsync(user, userRoles);

				await _userManager.AddToRolesAsync(user, roles);
			}
		}

		public async Task<string[]> GetRolesToUser(string userIdOrName)
		{
			AppUser? user = await _userManager.FindByIdAsync(userIdOrName);
			if (user == null)
			{
				user = await _userManager.FindByNameAsync(userIdOrName);
			}
			if (user != null)
			{
				var userRoles = await _userManager.GetRolesAsync(user);
				return userRoles.ToArray();
			}
			return new string[] { };
		}

		public async Task<bool> HasRolePermissionToEndpointAsync(string userName, string code)
		{
			var userRoles = await GetRolesToUser(userName);
			if (!userRoles.Any())
				return false;

			Endpoint? endpoint = await _endpointReadRepository.Table
				.Include(e => e.Roles)
				 .FirstOrDefaultAsync(e => e.Code == code);

			if (endpoint == null)
				return false;

			var endpointRoles = endpoint.Roles.Select(r => r.Name);
			foreach(var userRole in userRoles)
			{
				foreach (var endpointRole in  endpointRoles)
					if (userRole == endpointRole)
						return true;
			}
			return false;
		}

		public int TotalUsersCount => _userManager.Users.Count();

	}
}
