namespace Corely.IAM.BasicAuths.Models
{
    public record VerifyBasicAuthRequest(
        int UserId,
        string Password);
}
