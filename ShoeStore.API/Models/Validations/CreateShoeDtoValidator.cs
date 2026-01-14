using FluentValidation;
using ShoeStore.API.Models.DTOs;

namespace ShoeStore.API.Models.Validations
{
    public class ShoeCreateDtoValidator : AbstractValidator<ShoeCreateDto>
    {
        public ShoeCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Shoe name is required")
                .MaximumLength(100);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");

            RuleFor(x => x.Size)
                .InclusiveBetween(35, 50)
                .WithMessage("Size must be between 35 and 50");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Color is required");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
