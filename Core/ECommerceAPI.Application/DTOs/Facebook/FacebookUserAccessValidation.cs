using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Facebook
{
	public class FacebookUserAccessValidation
	{
        [JsonPropertyName("data")]
        public FacebookUserAccessValidationData Data { get; set; }
    }
	public class FacebookUserAccessValidationData {

		[JsonPropertyName("user_id")]
		public string UserId { get; set; }
		[JsonPropertyName("is_valid")]
		public bool isValid { get; set; }
	}

}
