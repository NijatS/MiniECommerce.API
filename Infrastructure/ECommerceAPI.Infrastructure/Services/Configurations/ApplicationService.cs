using ECommerceAPI.Application.Abstractions.Services.Configurations;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.DTOs.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Configurations
{
	public class ApplicationService : IApplicationService
	{
		public List<Menu> GetAuthorizeDefinitionEndpoints(Type type)
		{
			Assembly assembly = Assembly.GetAssembly(type);
			var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

			if(controllers != null) {

				foreach (var controller in controllers)
				{
					var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute)));

					if(actions != null)
					{
						foreach (var action in actions)
						{
							action.GetCustomAttributes(true);
						}
					}
				}
			}

			

			return null;
		}
	}
}
