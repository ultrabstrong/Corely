using Corely.IAM.Accounts.Models;
using Corely.IAM.Models;

namespace Corely.IAM.Accounts.Services
{
    internal interface IAccountService
    {
        Task<CreateResult> CreateAccountAsync(CreateAccountRequest createAccountRequest);
        Task<Account?> GetAccountAsync(int accountId);
        Task<Account?> GetAccountAsync(string accountName);
    }
}
