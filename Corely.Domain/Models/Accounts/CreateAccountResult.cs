namespace Corely.Domain.Models.Accounts
{
    public record CreateAccountResult(
        bool IsSuccess,
        string? Message,
        int CreatedId)
        : CreatedResultBase(IsSuccess, Message, CreatedId)
    {
    }
}
