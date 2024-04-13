namespace Corely.Domain.Models.Auth
{
    public record UpsertBasicAuthRequest(
        int UserId,
        string Password)
    { }
}
