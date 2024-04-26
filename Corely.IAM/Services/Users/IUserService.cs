using Corely.IAM.Models;
using Corely.IAM.Models.Users;

namespace Corely.IAM.Services.Users
{
    public interface IUserService
    {
        Task<CreateResult> CreateUserAsync(CreateUserRequest createUserRequest);

    }
}
