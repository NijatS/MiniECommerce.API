using ECommerceAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
	public interface IRoleService
	{
		Task<bool> CreateRole(string name);
		Task<bool> UpdateRole(string id ,string name);
		Task<bool> DeleteRole(string id);
		(object,int) GetAllRoles(int page,int size);
		Task<(string id,string name)> GetRoleById(string id);

	}
}
