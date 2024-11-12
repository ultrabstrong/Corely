using Corely.IAM.Auth.Models;

namespace Corely.IAM.Auth.Services
{
    internal interface IAuthService
    {
        Task<UpsertBasicAuthResult> UpsertBasicAuthAsync(UpsertBasicAuthRequest request);
        Task<bool> VerifyBasicAuthAsync(VerifyBasicAuthRequest request);
    }
}
