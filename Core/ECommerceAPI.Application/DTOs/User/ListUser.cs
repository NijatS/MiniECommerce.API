﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.User
{
	public class ListUser
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string FullName { get; set; }
		public bool TwoFactor { get; set; }
	}
}
