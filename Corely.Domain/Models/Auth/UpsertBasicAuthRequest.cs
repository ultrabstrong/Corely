namespace Corely.Domain.Models.Auth
{
    public record UpsertBasicAuthRequest(
        int UserId,
        string Username,
        string Password)
    { }
}
