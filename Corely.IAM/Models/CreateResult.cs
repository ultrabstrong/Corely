namespace Corely.IAM.Models
{
    internal record CreateResult(
        bool IsSuccess,
        string? Message,
        int CreatedId)
        : ResultBase(IsSuccess, Message);
}
