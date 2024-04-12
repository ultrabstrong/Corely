namespace Corely.Domain.Models.AccountManagement
{
    public record RegisterRequest(
        string AccountName,
        string Username,
        string Email,
        string Password)
    {
    }
}
