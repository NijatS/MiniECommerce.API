using ECommerceAPI.Application.Features.Commands.Order.CompleteOrder;
using ECommerceAPI.Application.Features.Commands.Order.CreateOrder;
using ECommerceAPI.Application.Features.Queries.Order.GetAllOrders;
using ECommerceAPI.Application.Features.Queries.Order.GetOrderById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Admin")]
	public class OrdersController : ControllerBase
	{
		readonly IMediator _mediator;

		public OrdersController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest request)
		{
			CreateOrderCommandResponse response = await _mediator.Send(request);

			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest request)
		{
			GetAllOrdersQueryResponse response = await _mediator.Send(request);

			return Ok(response);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrderById([FromRoute] GetOrderByIdQueryRequest request)
		{
			GetOrderByIdQueryResponse response = await _mediator.Send(request);

			return Ok(response);
		}
		[HttpGet("complete-order/{Id}")]
		public async Task<IActionResult> CompleteOrder([FromRoute] CompleteOrderCommandRequest request)
		{
			CompleteOrderCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
