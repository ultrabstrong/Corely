using Corely.Domain.Models;
using Corely.Domain.Models.Accounts;

namespace Corely.Domain.Services.Accounts
{
    public interface IAccountService
    {
        Task<CreateResult> CreateAccountAsync(CreateAccountRequest createAccountRequest);
    }
}
