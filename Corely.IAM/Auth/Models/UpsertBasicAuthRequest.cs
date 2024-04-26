namespace Corely.IAM.Auth.Models
{
    public record UpsertBasicAuthRequest(
        int UserId,
        string Password)
    { }
}
