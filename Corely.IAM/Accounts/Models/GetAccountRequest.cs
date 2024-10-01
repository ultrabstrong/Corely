namespace Corely.IAM.Accounts.Models
{
    public record GetAccountRequest
    {
        private GetAccountRequest(int accountId)
        {
            AccountId = accountId;
        }

        private GetAccountRequest(string accountName)
        {
            AccountName = accountName;
        }

        public static GetAccountRequest ForAccountId(int accountId) => new(accountId);

        public static GetAccountRequest ForAccountName(string accountName) => new(accountName);

        public int? AccountId { get; init; }

        public string? AccountName { get; init; }
    }
}
