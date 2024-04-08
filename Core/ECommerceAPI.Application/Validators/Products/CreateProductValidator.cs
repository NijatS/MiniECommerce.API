using ECommerceAPI.Application.Features.Commands.Product.CreateProduct;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommandRequest>
	{
        public CreateProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty()
                .NotNull()
                  .WithMessage("Please fill name field")
                .MaximumLength(150)
                .MinimumLength(5);

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                .Must(s => s >= 0);

			RuleFor(p => p.Price)
			   .NotEmpty()
			   .NotNull()
			   .Must(p => p >= 0);

		}
    }
}
