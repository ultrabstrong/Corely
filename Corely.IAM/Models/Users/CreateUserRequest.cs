namespace Corely.IAM.Models.Users
{
    public record CreateUserRequest(
        int AccountId,
        string Username,
        string Email)
    { }
}
