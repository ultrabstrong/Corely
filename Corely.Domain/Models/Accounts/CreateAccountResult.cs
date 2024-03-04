using Corely.Domain.Entities.Accounts;

namespace Corely.Domain.Models.Accounts
{
    public record CreateAccountResult : CreateResult
    {
        internal CreateAccountResult(
            bool isSuccessful,
            string message,
            AccountEntity accountEntity)
        : base(isSuccessful, message, accountEntity.Id)
        {
            AccountEntity = accountEntity;
        }

        internal AccountEntity AccountEntity { get; init; }
    }
}
