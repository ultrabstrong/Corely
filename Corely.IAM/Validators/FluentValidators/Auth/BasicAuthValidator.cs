using Corely.IAM.BasicAuths.Constants;
using Corely.IAM.BasicAuths.Models;
using FluentValidation;

namespace Corely.IAM.Validators.FluentValidators.Auth
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
