using ECommerceAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services
{
	public class MailService : IMailService
	{
		readonly IConfiguration _configuration;

		public MailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
		{
			await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
		}

		public async Task SendMailAsync(string[] toes, string subject, string body, bool isBodyHtml = true)
		{
			MailMessage message = new();
			message.Subject = subject;
			message.Body = body;
			message.IsBodyHtml = isBodyHtml;
			foreach (var to in toes)
				message.To.Add(to);

			message.From = new MailAddress(_configuration["Mail:UserName"], "NJ ECommerce",System.Text.Encoding.UTF8);

			SmtpClient smtp = new();
			smtp.Port = 587;
			smtp.EnableSsl = true;
			smtp.UseDefaultCredentials = false;
			smtp.Credentials = new NetworkCredential(_configuration["Mail:UserName"], _configuration["Mail:Password"]);

			smtp.Host = _configuration["Mail:Host"];

			await smtp.SendMailAsync(message);
		}

		public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string fullName)
		{
			string mail = $"Hello, Dear {fullName}<br>" +
				$"Your order with the following code {orderCode} that you ordered on {orderDate} has been shipped.";

			await SendMailAsync(to, $"Completed No:{orderCode} order", mail, true);
		}


		public async Task SendPasswordResetMailAsync(string to,string userId,string resetToken)
		{
			string mail = @"Hello<br>If you want to reset password, please click on the link below." +
				"<br><strong><a target='_blank' href=";

			mail += _configuration["AngularClientUrl"] + "/update-password/" + userId + "/" + resetToken;

			mail += ">Click this link...</a></strong><br><br><span style=\"font-size:12px;\">If this request has not been fulfilled by you, please do not take this e-mail seriously.</span><br>Best Regards... <br><br><br>NJ ECommerce";

			await SendMailAsync(to, "Reset Password", mail, true);
		}
	}
}
