using ECommerceAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUsers.Login
{
	public class LoginCommandResponse
	{
		
	}
	public class LoginCommandSuccessResponse : LoginCommandResponse
	{
		public Token Token { get; set; }

	}
	public class LoginCommandErrorResponse : LoginCommandResponse
	{
		public string Message { get; set; }

	}
}
