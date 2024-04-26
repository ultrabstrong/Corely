namespace Corely.IAM.Users.Models
{
    public record CreateUserRequest(
        int AccountId,
        string Username,
        string Email)
    { }
}
