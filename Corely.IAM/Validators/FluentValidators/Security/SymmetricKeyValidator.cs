using Corely.IAM.Security;
using FluentValidation;

namespace Corely.IAM.Validators.FluentValidators.Security
{
    internal class SymmetricKeyValidator : AbstractValidator<SymmetricKey>
    {
        public SymmetricKeyValidator()
        {
            RuleFor(m => m.Key)
                .NotNull()
                .NotEmpty()
                .MaximumLength(SymmetricKeyConstants.KEY_MAX_LENGTH);

            RuleFor(m => m.Version)
                .GreaterThanOrEqualTo(SymmetricKeyConstants.VERSION_MIN_VALUE);
        }
    }
}
