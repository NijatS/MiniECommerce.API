using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.AppUser.GetRolesToUser
{
	public class GetRolesToUserQueryHandler : IRequestHandler<GetRolesToUserQueryRequest, GetRolesToUserQueryResponse>
	{ 
		readonly IUserService _userService;

		public GetRolesToUserQueryHandler(IUserService userService)
		{
			_userService = userService;
		}

		public async Task<GetRolesToUserQueryResponse> Handle(GetRolesToUserQueryRequest request, CancellationToken cancellationToken)
		{
			var data = await _userService.GetRolesToUser(request.UserId);
			return new()
			{
				Roles = data
			};
		}
	}
}
