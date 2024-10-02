namespace Corely.IAM.Models
{
    public record RegisterRequest(
        string AccountName,
        string Username,
        string Email,
        string Password);
}
