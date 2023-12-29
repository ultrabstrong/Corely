using Corely.Domain.Constants.Auth;
using Corely.Domain.Models.Auth;
using FluentValidation;

namespace Corely.Domain.Validators.FluentValidators.Auth
{
    internal class BasicAuthValidator : AbstractValidator<BasicAuth>
    {
        public BasicAuthValidator()
        {
            RuleFor(m => m.Username).NotEmpty().MaximumLength(BasicAuthConstants.USERNAME_MAX_LENGTH);
            RuleFor(m => m.Password).NotEmpty();
            RuleFor(m => m.Password.Hash).NotEmpty().MaximumLength(BasicAuthConstants.PASSWORD_MAX_LENGTH);
        }
    }
}
