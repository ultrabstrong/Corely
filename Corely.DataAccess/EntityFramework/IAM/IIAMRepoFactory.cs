using Corely.IAM.Accounts.Entities;
using Corely.IAM.Auth.Entities;
using Corely.IAM.Repos;
using Corely.IAM.Users.Entities;

namespace Corely.DataAccess.EntityFramework.IAM
{
    public interface IIAMRepoFactory
    {
        IRepoExtendedGet<AccountEntity> CreateAccountRepo();

        IReadonlyRepo<AccountEntity> CreateReadonlyAccountRepo();

        IRepoExtendedGet<UserEntity> CreateUserRepo();

        IReadonlyRepo<UserEntity> CreateReadonlyUserRepo();

        IRepoExtendedGet<BasicAuthEntity> CreateBasicAuthRepo();

        IUnitOfWorkProvider CreateUnitOfWorkProvider();
    }
}
