using Corely.IAM.Entities.Accounts;
using Corely.IAM.Entities.Auth;
using Corely.IAM.Entities.Users;
using Corely.IAM.Repos;

namespace Corely.DataAccess.Factories
{
    public interface IAccountManagementRepoFactory
    {
        IRepoExtendedGet<AccountEntity> CreateAccountRepo();

        IReadonlyRepo<AccountEntity> CreateReadonlyAccountRepo();

        IRepoExtendedGet<UserEntity> CreateUserRepo();

        IRepoExtendedGet<BasicAuthEntity> CreateBasicAuthRepo();

        IUnitOfWorkProvider CreateUnitOfWorkProvider();
    }
}
