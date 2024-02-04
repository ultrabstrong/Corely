namespace Corely.Domain.Models.AccountManagement
{
    public record SignUpRequest(
        string AccountName,
        string Username,
        string Email,
        string Password)
    {
    }
}
