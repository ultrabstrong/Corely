using Corely.IAM.BasicAuths.Constants;
using FluentValidation;

namespace Corely.IAM.Validators.FluentValidators.BasicAuth
{
    internal class BasicAuthValidator : AbstractValidator<BasicAuths.Models.BasicAuth>
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
