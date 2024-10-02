namespace Corely.IAM.Models
{
    public record RegisterResult(
        bool IsSuccess,
        string? Message,
        int CreatedAccountId,
        int CreatedUserId,
        int CreatedAuthId)
        : ResultBase(IsSuccess, Message);
}
