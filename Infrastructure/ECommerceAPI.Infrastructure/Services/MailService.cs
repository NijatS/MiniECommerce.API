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

		public async Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml = true)
		{
			await SendMessageAsync(new[] { to }, subject, body, isBodyHtml);
		}

		public async Task SendMessageAsync(string[] toes, string subject, string body, bool isBodyHtml = true)
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
	}
}
