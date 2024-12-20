using Corely.IAM.Accounts.Models;

namespace Corely.IAM.Accounts.Processors;

internal interface IAccountProcessor
{
    Task<CreateAccountResult> CreateAccountAsync(CreateAccountRequest createAccountRequest);
    Task<Account?> GetAccountAsync(int accountId);
    Task<Account?> GetAccountAsync(string accountName);
}
