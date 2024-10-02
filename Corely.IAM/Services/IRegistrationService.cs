using Corely.IAM.Models;

namespace Corely.IAM.Services
{
    public interface IRegistrationService
    {
        Task<RegisterUserResult> RegisterUserAsync(RegisterUserRequest request);

        Task<RegisterAccountResult> RegisterAccountAsync(RegisterAccountRequest request);
    }
}
