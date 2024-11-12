namespace Corely.IAM.BasicAuths.Models
{
    public record UpsertBasicAuthRequest(
        int UserId,
        string Password);
}
