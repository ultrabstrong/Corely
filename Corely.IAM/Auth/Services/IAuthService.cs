using Corely.IAM.Auth.Models;

namespace Corely.IAM.Auth.Services
{
    public interface IAuthService
    {
        public Task<UpsertBasicAuthResult> UpsertBasicAuthAsync(UpsertBasicAuthRequest request);
    }
}
