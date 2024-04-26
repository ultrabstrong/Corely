using Corely.IAM.AccountManagement.Models;

namespace Corely.IAM.AccountManagement.Services
{
    public interface IAccountManagementService
    {
        Task<RegisterResult> RegisterAsync(RegisterRequest request);
    }
}
