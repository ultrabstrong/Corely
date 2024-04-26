using Corely.IAM.Constants.Accounts;
using Corely.IAM.Models.Accounts;
using FluentValidation;

namespace Corely.IAM.Validators.FluentValidators.Accounts
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
