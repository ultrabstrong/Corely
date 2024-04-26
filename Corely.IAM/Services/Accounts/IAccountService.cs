using Corely.IAM.Models;
using Corely.IAM.Models.Accounts;

namespace Corely.IAM.Services.Accounts
{
    public interface IAccountService
    {
        Task<CreateResult> CreateAccountAsync(CreateAccountRequest createAccountRequest);
    }
}
