using Corely.IAM.Security.Constants;
using Corely.IAM.Security.Models;
using FluentValidation;

namespace Corely.IAM.Validators.FluentValidators.Security
{
    internal class SymmetricKeyValidator : AbstractValidator<SymmetricKey>
    {
        public SymmetricKeyValidator()
        {
            RuleFor(m => m.Key)
                .NotNull();

            RuleFor(m => m.Key.Secret)
                .NotEmpty()
                .MaximumLength(SymmetricKeyConstants.KEY_MAX_LENGTH);

            RuleFor(m => m.Version)
                .GreaterThanOrEqualTo(SymmetricKeyConstants.VERSION_MIN_VALUE);
        }
    }
}
