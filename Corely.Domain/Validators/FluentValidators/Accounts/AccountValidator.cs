using Corely.Domain.Constants.Accounts;
using Corely.Domain.Models.Accounts;
using FluentValidation;

namespace Corely.Domain.Validators.FluentValidators.Accounts
{
    internal class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(x => x.AccountName)
                .NotEmpty()
                .MinimumLength(AccountConstants.ACCOUNT_NAME_MIN_LENGTH)
                .MaximumLength(AccountConstants.ACCOUNT_NAME_MAX_LENGTH);
        }
    }
}
