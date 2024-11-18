namespace Corely.IAM.Models
{
    public record RegisterGroupResult(
        bool IsSuccess,
        string? Message,
        int CreatedGroupId)
        : ResultBase(IsSuccess, Message);
}
