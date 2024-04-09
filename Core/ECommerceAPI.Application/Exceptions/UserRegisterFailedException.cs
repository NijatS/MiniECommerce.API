using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Exceptions
{
	public class UserRegisterFailedException : Exception
	{
		public UserRegisterFailedException() : base("The user encountered an error while registering")
		{
		}

		public UserRegisterFailedException(string? message) : base(message)
		{
		}

		public UserRegisterFailedException(string? message, Exception? innerException) : base(message, innerException)
		{
		}
	}
}
