﻿using ECommerceAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUsers.FacebookLogin
{
	public class FacebookLoginCommandResponse
	{
		public Token Token { get; set; }
	}
}