using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Services.Configurations;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services
{
	public class AuthorizationEndpointService : IAuthorizationEndpointService
	{
		readonly IApplicationService _applicationService;
		readonly IEndpointReadRepository _endpointReadRepository;
		readonly IEndpointWriteRepository _endpointWriteRepository;
		readonly IMenuReadRepository _menuReadRepository;
		readonly IMenuWriteRepository _menuWriteRepository;
		readonly RoleManager<AppRole> _roleManager;

		public AuthorizationEndpointService(IApplicationService applicationService, IEndpointReadRepository endpointReadRepository, IEndpointWriteRepository endpointWriteRepository, IMenuReadRepository menuReadRepository, IMenuWriteRepository menuWriteRepository, RoleManager<AppRole> roleManager)
		{
			_applicationService = applicationService;
			_endpointReadRepository = endpointReadRepository;
			_endpointWriteRepository = endpointWriteRepository;
			_menuReadRepository = menuReadRepository;
			_menuWriteRepository = menuWriteRepository;
			_roleManager = roleManager;
		}

		public async Task AssignRoleEndpointAsync(string[] roles, string code, string menu, Type type)
		{
			Menu? _menu = await _menuReadRepository.GetSingleAsync(m => m.Name == menu);

			if (_menu == null)
			{
				await _menuWriteRepository.AddAsync(new()
				{
					Id = Guid.NewGuid(),
					Name = menu
				});
				await _menuWriteRepository.SaveAsync();
			}

			Endpoint? endpoint = await _endpointReadRepository.Table.Include(e => e.Menu)
			 .FirstOrDefaultAsync(e => e.Menu.Name == menu && e.Menu.Name == menu);
			if (endpoint == null)
			{
				var action = _applicationService.GetAuthorizeDefinitionEndpoints(type).FirstOrDefault(e => e.Name == menu)
					?.Actions.FirstOrDefault(e => e.Code == code);

				endpoint = new()
				{
					Code = action.Code,
					ActionType = action.ActionType,
					HttpType = action.HttpType,
					Definition = action.Definition,
					Id = Guid.NewGuid(),
				};

				await _endpointWriteRepository.AddAsync(endpoint);
				await _endpointWriteRepository.SaveAsync();

			}
			foreach(var role in roles)
			{
				AppRole? findedRole = await _roleManager.FindByNameAsync(role);

				if (findedRole != null)
				{
					endpoint.Roles.Add(findedRole);
				}
			}
			await _endpointWriteRepository.SaveAsync();

		}
	}
}
