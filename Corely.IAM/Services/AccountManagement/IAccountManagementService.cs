using Corely.IAM.Models.AccountManagement;

namespace Corely.IAM.Services.AccountManagement
{
    public interface IAccountManagementService
    {
        Task<RegisterResult> RegisterAsync(RegisterRequest request);
    }
}
