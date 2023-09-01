using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Factories.AccountManagement
{
    public interface IAccountManagementRepoFactory
    {
        IUserRepo GetUserRepo();

        IAuthRepo<BasicAuthEntity> GetBasicAuthRepo();
    }
}
