namespace Corely.IAM.Accounts.Models
{
    public record CreateAccountRequest(
        string AccountName,
        int UserIdOfOwner);
}
