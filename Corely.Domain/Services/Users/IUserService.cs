using Corely.Domain.Models.Auth;
using Corely.Domain.Models.Users;

namespace Corely.Domain.Services.Users
{
    public interface IUserService
    {
        void Create(User user, BasicAuth basicAuth);

    }
}
