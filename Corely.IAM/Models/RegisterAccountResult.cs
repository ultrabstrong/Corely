using Corely.IAM.Accounts.Models;

namespace Corely.IAM.Models;

public record RegisterAccountResult(
    CreateAccountResultCode ResultCode,
    string? Message,
    int CreatedAccountId);
