namespace Corely.IAM.Models.AccountManagement
{
    public record SignInRequest(
        string Username,
        string Password)
    {
    }
}
