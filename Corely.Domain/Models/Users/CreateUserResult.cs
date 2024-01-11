namespace Corely.Domain.Models.Users
{
    public record CreateUserResult : ResultBase
    {
        public CreateUserResult(bool IsSuccess, string? Message)
            : base(IsSuccess, Message) { }
    }
}
