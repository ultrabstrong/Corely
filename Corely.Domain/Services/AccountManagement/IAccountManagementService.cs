using Corely.Domain.Models.AccountManagement;

namespace Corely.Domain.Services.AccountManagement
{
    public interface IAccountManagementService
    {
        Task<RegisterResult> RegisterAsync(RegisterRequest request);
    }
}
