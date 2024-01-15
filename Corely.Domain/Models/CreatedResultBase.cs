namespace Corely.Domain.Models
{
    public abstract record CreatedResultBase(
        bool IsSuccess,
        string? Message,
        int CreatedId)
        : ResultBase(IsSuccess, Message)
    {
    }
}
