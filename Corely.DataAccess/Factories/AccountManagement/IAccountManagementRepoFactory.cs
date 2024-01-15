using Corely.Domain.Entities.Accounts;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Factories.AccountManagement
{
    public interface IAccountManagementRepoFactory
    {
        IRepoExtendedGet<AccountEntity> CreateAccountRepo();

        IRepoExtendedGet<UserEntity> CreateUserRepo();

        IRepoExtendedGet<BasicAuthEntity> CreateBasicAuthRepo();
    }
}
