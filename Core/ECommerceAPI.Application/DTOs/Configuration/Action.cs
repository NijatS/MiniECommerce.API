﻿using ECommerceAPI.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Configuration
{
	public class Action
	{
		public ActionType ActionType { get; set; }
		public string HttpType { get; set; }
		public string Definition { get; set; }
	}
}