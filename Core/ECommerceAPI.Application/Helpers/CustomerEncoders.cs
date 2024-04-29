using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Helpers
{
	public static class CustomerEncoders
	{
		public static string UrlEncode(this string token)
		{
			byte[] tokenByte = Encoding.UTF8.GetBytes(token);
			return WebEncoders.Base64UrlEncode(tokenByte);
		}
		public static string UrlDecode(this string token)
		{
			byte[] tokenBytes = WebEncoders.Base64UrlDecode(token);
			return Encoding.UTF8.GetString(tokenBytes);
		}
	}
}
