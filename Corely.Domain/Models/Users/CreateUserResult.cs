namespace Corely.Domain.Models.Users
{
    public record CreateUserResult(
        bool IsSuccess,
        string? Message,
        int CreatedId)
        : CreatedResultBase(IsSuccess, Message, CreatedId)
    { }
}
