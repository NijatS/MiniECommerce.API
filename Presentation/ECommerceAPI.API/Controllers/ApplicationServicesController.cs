using ECommerceAPI.Application.Abstractions.Services.Configurations;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes ="Admin")]
	public class ApplicationServicesController : ControllerBase
	{
		readonly IApplicationService _applicationService;

		public ApplicationServicesController(IApplicationService applicationService)
		{
			_applicationService = applicationService;
		}
		[HttpGet]
		[AuthorizeDefinition(ActionType =ActionType.Reading,Definition ="Get Authorize Definition Endpoints",Menu ="Application Services")]
		public IActionResult GetAuthorizeDefinitionEndpoints()
		{
			var data = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));
			return Ok(data);
		}
	}
}
