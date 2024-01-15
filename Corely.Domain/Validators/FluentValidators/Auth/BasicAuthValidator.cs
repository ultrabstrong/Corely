using Corely.Domain.Constants.Auth;
using Corely.Domain.Models.Auth;
using FluentValidation;

namespace Corely.Domain.Validators.FluentValidators.Auth
{
    internal class BasicAuthValidator : AbstractValidator<BasicAuth>
    {
        public BasicAuthValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(m => m.Username)
                .NotEmpty()
                .MinimumLength(BasicAuthConstants.USERNAME_MIN_LENGTH)
                .MaximumLength(BasicAuthConstants.USERNAME_MAX_LENGTH);
            RuleFor(m => m.Password)
                .NotNull();
            RuleFor(m => m.Password.Hash)
                .NotEmpty()
                .MaximumLength(BasicAuthConstants.PASSWORD_MAX_LENGTH);
        }
    }
}
