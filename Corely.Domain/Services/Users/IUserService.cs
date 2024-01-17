using Corely.Domain.Models;
using Corely.Domain.Models.Users;

namespace Corely.Domain.Services.Users
{
    public interface IUserService
    {
        Task<CreateResult> CreateUserAsync(CreateUserRequest createUserRequest);

    }
}
