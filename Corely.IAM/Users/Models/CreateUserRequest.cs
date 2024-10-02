namespace Corely.IAM.Users.Models
{
    public record CreateUserRequest(
        string Username,
        string Email);
}
