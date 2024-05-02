﻿using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Role.GetRoleById
{
	public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQueryRequest, GetRoleByIdQueryResponse>
	{
		readonly IRoleService _roleService;

		public GetRoleByIdQueryHandler(IRoleService roleService)
		{
			_roleService = roleService;
		}

		public async Task<GetRoleByIdQueryResponse> Handle(GetRoleByIdQueryRequest request, CancellationToken cancellationToken)
		{
			(string id, string name) = await _roleService.GetRoleById(request.Id);

			return new() { Id = id, Name = name };
		}
	}
}
