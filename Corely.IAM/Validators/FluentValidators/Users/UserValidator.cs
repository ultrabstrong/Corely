using Corely.IAM.Constants.Users;
using Corely.IAM.Models.Users;
using FluentValidation;

namespace Corely.IAM.Validators.FluentValidators.Users
{
    internal class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(m => m.Username)
                .NotEmpty()
                .MinimumLength(UserConstants.USERNAME_MIN_LENGTH)
                .MaximumLength(UserConstants.USERNAME_MAX_LENGTH);
            RuleFor(m => m.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(UserConstants.EMAIL_MAX_LENGTH);
        }
    }
}
