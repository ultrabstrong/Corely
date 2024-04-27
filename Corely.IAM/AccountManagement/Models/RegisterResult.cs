using Corely.IAM.Models;

namespace Corely.IAM.AccountManagement.Models
{
    public record RegisterResult(
        bool IsSuccess,
        string? Message,
        int CreatedAccountId,
        int CreatedUserId,
        int CreatedAuthId)
        : ResultBase(IsSuccess, Message);
}
