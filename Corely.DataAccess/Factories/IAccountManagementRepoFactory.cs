using Corely.IAM.Accounts.Entities;
using Corely.IAM.Auth.Entities;
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
