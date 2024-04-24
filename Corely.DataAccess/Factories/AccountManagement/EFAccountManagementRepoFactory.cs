using Corely.DataAccess.Connections;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.DataSources.Mock;
using Corely.DataAccess.Repos;
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
        private readonly bool _supportsTransactions;

        private static string[] ProvidersWithoutTransactionSupport =>
        [
            "Microsoft.EntityFrameworkCore.InMemory",
        ];

        public EFAccountManagementRepoFactory(
            ILoggerFactory loggerFactory,
            EFConnection connection)
        {
            _loggerFactory = loggerFactory;
            _connection = connection;
            _accountManagementDbContext = CreateDbContext();

            _supportsTransactions = !ProvidersWithoutTransactionSupport
                .Contains(_accountManagementDbContext.Database.ProviderName);
        }

        internal virtual AccountManagementDbContext CreateDbContext()
        {
            return new(_connection.Configuration);
        }

        public IRepoExtendedGet<AccountEntity> CreateAccountRepo()
        {
            return new EFRepoExtendedGet<AccountEntity>(
                _loggerFactory.CreateLogger<EFRepoExtendedGet<AccountEntity>>(),
                _accountManagementDbContext,
                _accountManagementDbContext.Accounts);
        }

        public IReadonlyRepo<AccountEntity> CreateReadonlyAccountRepo()
        {
            return new EFReadonlyRepo<AccountEntity>(
                _loggerFactory.CreateLogger<EFReadonlyRepo<AccountEntity>>(),
                _accountManagementDbContext.Accounts);
        }

        public IRepoExtendedGet<UserEntity> CreateUserRepo()
        {
            return new EFRepoExtendedGet<UserEntity>(
                _loggerFactory.CreateLogger<EFRepoExtendedGet<UserEntity>>(),
                _accountManagementDbContext,
                _accountManagementDbContext.Users);
        }

        public IRepoExtendedGet<BasicAuthEntity> CreateBasicAuthRepo()
        {
            return new EFRepoExtendedGet<BasicAuthEntity>(
                _loggerFactory.CreateLogger<EFRepoExtendedGet<BasicAuthEntity>>(),
                _accountManagementDbContext,
                _accountManagementDbContext.BasicAuths);
        }

        public IUnitOfWorkProvider CreateUnitOfWorkProvider()
        {
            return _supportsTransactions
                ? new EFUoWProvider(_accountManagementDbContext)
                : new MockUoWProvider();
        }
    }
}
