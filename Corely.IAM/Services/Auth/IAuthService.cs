using Corely.IAM.Models.Auth;

namespace Corely.IAM.Services.Auth
{
    public interface IAuthService
    {
        public Task<UpsertBasicAuthResult> UpsertBasicAuthAsync(UpsertBasicAuthRequest request);
    }
}
