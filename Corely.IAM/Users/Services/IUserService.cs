using Corely.IAM.Models;
using Corely.IAM.Users.Models;

namespace Corely.IAM.Users.Services
{
    public interface IUserService
    {
        Task<CreateResult> CreateUserAsync(CreateUserRequest createUserRequest);
        Task<User?> GetUserAsync(int userId);
        Task<User?> GetUserAsync(string userName);
        Task UpdateUserAsync(User user);
        Task<string?> GetUserAuthTokenAsync(int userId);
        Task<bool> IsUserAuthTokenValidAsync(int userId, string authToken);
        Task<string?> GetAsymmetricSignatureVerificationKeyAsync(int userId);
    }
}
