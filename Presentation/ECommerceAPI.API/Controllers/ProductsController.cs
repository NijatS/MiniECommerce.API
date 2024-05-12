using ECommerceAPI.Application.Consts;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Features.Commands.Product.CreateProduct;
using ECommerceAPI.Application.Features.Commands.Product.RemoveProduct;
using ECommerceAPI.Application.Features.Commands.Product.UpdateProduct;
using ECommerceAPI.Application.Features.Commands.Product.UpdateStockQrCodeToProduct;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.ChangeShowCaseImage;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.DeleteProductImage;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ECommerceAPI.Application.Features.Queries.Product.GetAllProduct;
using ECommerceAPI.Application.Features.Queries.Product.GetByIdProduct;
using ECommerceAPI.Application.Features.Queries.Product.QRCode;
using ECommerceAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerceAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
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
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Create Product")]

		public async Task<IActionResult> Post([FromBody]CreateProductCommandRequest createProductCommandRequest)
		{
			CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
			return StatusCode((int)HttpStatusCode.Created);
		}

		[HttpPut]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Update Product")]
		public async Task<IActionResult> Put([FromBody]UpdateProductCommandRequest updateProductCommandRequest)
		{
		    UpdateProductCommandResponse response =  await _mediator.Send(updateProductCommandRequest);
			return Ok();
		}

		[HttpDelete("{Id}")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Remove Product")]
		public async Task<IActionResult> Delete([FromRoute]RemoveProductCommandRequest removeProductCommandRequest)
		{
			RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);
			return Ok();
		}

		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Upload Product File")]
		public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
		{
			uploadProductImageCommandRequest.Files = Request.Form.Files; 
		    UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);
			return Ok();
		}

		[HttpGet("[action]/{Id}")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Reading, Definition = "Get Products Images")]
		public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest productImagesQueryRequest)
		{
	       List<GetProductImagesQueryResponse> response =  await _mediator.Send(productImagesQueryRequest);
			return Ok(response);

		}

		[HttpDelete("[action]/{Id}")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product Image")]
		public async Task<IActionResult> DeleteProductImage([FromRoute]DeleteProductImageCommandRequest deleteProductImageCommandRequest, [FromQuery] string imageId)
		{
			deleteProductImageCommandRequest.ImageId = imageId;
			DeleteProductImageCommandResponse response = await _mediator.Send(deleteProductImageCommandRequest);
			return Ok();
		}
		[HttpGet("[action]")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Change Showcase Image")]
		public async Task<IActionResult> ChangeShowcaseImage([FromQuery]ChangeShowCaseImageCommandRequest request)
		{
			ChangeShowCaseImageCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}
		[HttpGet("qrcode/{ProductId}")]
		public async Task<IActionResult> GetQRCodeToProduct([FromRoute] QRCodeQueryRequest request)
		{
			QRCodeQueryResponse response = await _mediator.Send(request);
			return File(response.bytes,"image/png");
		}
		[HttpPut("qrcode")]
		public async Task<IActionResult> UpdateStockQrCodeToProduct([FromBody] UpdateStockQrCodeToProductCommandRequest request)
		{
			UpdateStockQrCodeToProductCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}

	}
}
