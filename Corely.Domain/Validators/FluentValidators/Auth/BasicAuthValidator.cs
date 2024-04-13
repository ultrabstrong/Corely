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
            RuleFor(m => m.Password)
                .NotNull();
            RuleFor(m => m.Password.Hash)
                .NotEmpty()
                .MaximumLength(BasicAuthConstants.PASSWORD_MAX_LENGTH);
        }
    }
}
