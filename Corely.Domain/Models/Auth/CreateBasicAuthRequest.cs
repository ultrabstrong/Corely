namespace Corely.Domain.Models.Auth
{
    public record CreateBasicAuthRequest(
        string Username,
        string Password)
    {
    }
}
