using FluentValidation;
using ShoeStore.API.Models.DTOs;

namespace ShoeStore.API.Models.Validations
{
    public class GoogleLoginDtoValidator : AbstractValidator<GoogleLoginDto>
    {
        public GoogleLoginDtoValidator()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty().WithMessage("Google ID token is required")
                .MinimumLength(100).WithMessage("Invalid Google ID token format");
        }
    }
}
