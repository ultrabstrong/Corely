using Corely.IAM.Models;

namespace Corely.IAM.Services
{
    public interface IRegistrationService
    {
        Task<RegisterResult> RegisterAsync(RegisterRequest request);
    }
}
