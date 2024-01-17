namespace Corely.Domain.Models
{
    public record CreateResult(
        bool IsSuccess,
        string? Message,
        int CreatedId)
        : ResultBase(IsSuccess, Message)
    {
    }
}
