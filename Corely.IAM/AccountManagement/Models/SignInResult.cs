using Corely.IAM.Models;

namespace Corely.IAM.AccountManagement.Models
{
    public record SignInResult(
        bool IsSuccess,
        string? Message,
        string? AuthToken)
        : ResultBase(IsSuccess, Message);
}
