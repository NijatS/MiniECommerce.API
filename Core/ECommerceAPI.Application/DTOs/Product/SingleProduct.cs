﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Product
{
	public class SingleProduct
	{
		public string Name { get; set; }
		public int Stock { get; set; }
		public double Price { get; set; }
	}
}
