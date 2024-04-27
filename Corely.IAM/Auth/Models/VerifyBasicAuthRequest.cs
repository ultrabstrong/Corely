namespace Corely.IAM.Auth.Models
{
    public record VerifyBasicAuthRequest(
        int UserId,
        string Password);
}
