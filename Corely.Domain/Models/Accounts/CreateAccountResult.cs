namespace Corely.Domain.Models.Accounts
{
    public record CreateAccountResult : ResultBase
    {
        public CreateAccountResult(bool IsSuccess, string? Message)
            : base(IsSuccess, Message)
        { }
    }
}
