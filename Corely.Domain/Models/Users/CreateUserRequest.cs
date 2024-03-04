using Corely.Domain.Entities.Accounts;

namespace Corely.Domain.Models.Users
{
    public record CreateUserRequest
    {
        public CreateUserRequest(
            int accountId,
            string username,
            string email)
        : this(new AccountEntity { Id = accountId }, username, email)
        {
        }

        internal CreateUserRequest(
            AccountEntity accountEntity,
            string username,
            string email)
        {
            AccountEntity = accountEntity;
            Username = username;
            Email = email;
        }

        internal AccountEntity AccountEntity { get; init; }

        public string Username { get; init; }

        public string Email { get; init; }
    }
}
