using Corely.DataAccess.Connections;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.Accounts;
using Corely.DataAccess.Repos.Auth;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Factories.AccountManagement
{
    internal class EFAccountManagementRepoFactory : IAccountManagementRepoFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly EFConnection _connection;
        private readonly AccountManagementDbContext _accountManagementDbContext;

        public EFAccountManagementRepoFactory(
            ILoggerFactory loggerFactory,
            EFConnection connection)
        {
            _loggerFactory = loggerFactory;
            _connection = connection;
            _accountManagementDbContext = CreateDbContext();
        }

        internal virtual AccountManagementDbContext CreateDbContext()
        {
            return new(_connection.Configuration);
        }

        public IRepoExtendedGet<AccountEntity> CreateAccountRepo()
        {
            return new EFAccountRepo(_loggerFactory.CreateLogger<EFAccountRepo>(), _accountManagementDbContext);
        }

        public IRepoExtendedGet<UserEntity> CreateUserRepo()
        {
            return new EFUserRepo(_loggerFactory.CreateLogger<EFUserRepo>(), _accountManagementDbContext);
        }

        public IRepoExtendedGet<BasicAuthEntity> CreateBasicAuthRepo()
        {
            return new EFBasicAuthRepo(_loggerFactory.CreateLogger<EFBasicAuthRepo>(), _accountManagementDbContext);
        }

        public IUnitOfWorkProvider CreateTransactionProvider()
        {
            return new EFAccountManagementUoWProvider(_accountManagementDbContext);
        }
    }
}
