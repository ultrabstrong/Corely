using Corely.Domain.Entities.Accounts;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;

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
