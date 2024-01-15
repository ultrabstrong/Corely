namespace Corely.Domain.Models.Users
{
    public record CreateUserRequest(
        string Username,
        string Email)
    { }
}
