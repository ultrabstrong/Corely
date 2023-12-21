using Corely.Domain.Constants.Users;
using Corely.Domain.Models.Users;
using FluentValidation;

namespace Corely.Domain.Validators.FluentValidators.Users
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(m => m.Username).NotEmpty().MaximumLength(UserConstants.USERNAME_MAX_LENGTH);
            RuleFor(m => m.Email).NotEmpty().EmailAddress().MaximumLength(UserConstants.EMAIL_MAX_LENGTH);
        }
    }
}
