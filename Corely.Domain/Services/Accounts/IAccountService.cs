using Corely.Domain.Models.Accounts;

namespace Corely.Domain.Services.Accounts
{
    public interface IAccountService
    {
        Task<CreateAccountResult> CreateAccountAsync(CreateAccountRequest createAccountRequest);
    }
}
