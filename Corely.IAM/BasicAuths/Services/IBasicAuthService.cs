using Corely.IAM.BasicAuths.Models;

namespace Corely.IAM.BasicAuths.Services
{
    internal interface IBasicAuthService
    {
        Task<UpsertBasicAuthResult> UpsertBasicAuthAsync(UpsertBasicAuthRequest request);
        Task<bool> VerifyBasicAuthAsync(VerifyBasicAuthRequest request);
    }
}
