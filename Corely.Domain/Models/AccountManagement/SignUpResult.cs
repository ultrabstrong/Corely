namespace Corely.Domain.Models.AccountManagement
{
    public record SignUpResult(
        bool IsSuccess,
        string? Message,
        int CreatedAccountId,
        int CreatedUserId,
        int CreatedAuthId)
        : ResultBase(IsSuccess, Message)
    {
    }
}
