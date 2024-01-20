using Corely.Domain.Models.Auth;

namespace Corely.Domain.Services.Auth
{
    public interface IAuthService
    {
        public Task<UpsertBasicAuthResult> UpsertBasicAuthAsync(UpsertBasicAuthRequest request);
    }
}
