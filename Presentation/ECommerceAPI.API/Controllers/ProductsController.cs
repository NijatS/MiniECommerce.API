﻿using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Features.Commands.Product.CreateProduct;
using ECommerceAPI.Application.Features.Commands.Product.RemoveProduct;
using ECommerceAPI.Application.Features.Commands.Product.UpdateProduct;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.DeleteProductImage;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ECommerceAPI.Application.Features.Queries.Product.GetAllProduct;
using ECommerceAPI.Application.Features.Queries.Product.GetByIdProduct;
using ECommerceAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerceAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Admin")]
	public class ProductsController : ControllerBase
	{
		readonly IMediator _mediator;
		public ProductsController(IMediator mediator)
		{
			_mediator = mediator;
		}
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
		{
			GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
			return Ok(response);
		}
		[HttpGet("{Id}")]
		public async Task<IActionResult> Get([FromRoute]GetByIdProductQueryRequest getByIdProductQueryRequest)
		{
			GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
			return Ok(response);
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]CreateProductCommandRequest createProductCommandRequest)
		{
			CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
			return StatusCode((int)HttpStatusCode.Created);
		}

		[HttpPut]
		public async Task<IActionResult> Put([FromBody]UpdateProductCommandRequest updateProductCommandRequest)
		{
		    UpdateProductCommandResponse response =  await _mediator.Send(updateProductCommandRequest);
			return Ok();
		}
		[HttpDelete("{Id}")]
		public async Task<IActionResult> Delete([FromRoute]RemoveProductCommandRequest removeProductCommandRequest)
		{
			RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);
			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
		{
			uploadProductImageCommandRequest.Files = Request.Form.Files; 
		    UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);
			return Ok();
		}
		[HttpGet("[action]/{Id}")]
		public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest productImagesQueryRequest)
		{
	       List<GetProductImagesQueryResponse> response =  await _mediator.Send(productImagesQueryRequest);
			return Ok(response);

		}

		[HttpDelete("[action]/{Id}")]
		public async Task<IActionResult> DeleteProductImage([FromRoute]DeleteProductImageCommandRequest deleteProductImageCommandRequest, [FromQuery] string imageId)
		{
			deleteProductImageCommandRequest.ImageId = imageId;
			DeleteProductImageCommandResponse response = await _mediator.Send(deleteProductImageCommandRequest);
			return Ok();
		}
	}
}
