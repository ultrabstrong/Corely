using Corely.IAM.Accounts.Models;
using Corely.IAM.Models;

namespace Corely.IAM.Accounts.Processors;

internal interface IAccountProcessor
{
    Task<CreateResult> CreateAccountAsync(CreateAccountRequest createAccountRequest);
    Task<Account?> GetAccountAsync(int accountId);
    Task<Account?> GetAccountAsync(string accountName);
}
