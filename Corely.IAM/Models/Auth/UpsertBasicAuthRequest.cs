namespace Corely.IAM.Models.Auth
{
    public record UpsertBasicAuthRequest(
        int UserId,
        string Password)
    { }
}
