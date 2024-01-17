using Corely.Domain.Models;
using Corely.Domain.Models.Auth;

namespace Corely.Domain.Services.Auth
{
    public interface IAuthService
    {
        public Task<CreateResult> AuthenticateAsync(CreateBasicAuthRequest request);
    }
}
