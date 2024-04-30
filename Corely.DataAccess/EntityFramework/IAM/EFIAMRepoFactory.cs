using Corely.DataAccess.EntityFramework.Repos;
using Corely.DataAccess.Mock;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Auth.Entities;
using Corely.IAM.Repos;
using Corely.IAM.Users.Entities;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.EntityFramework.IAM
{
    internal class EFIAMRepoFactory : IIAMRepoFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly EFConnection _connection;
        private readonly IAMDbContext _accountManagementDbContext;
        private readonly bool _supportsTransactions;

        private static string[] ProvidersWithoutTransactionSupport =>
        [
            "Microsoft.EntityFrameworkCore.InMemory",
        ];

        public EFIAMRepoFactory(
            ILoggerFactory loggerFactory,
            EFConnection connection)
        {
            _loggerFactory = loggerFactory;
            _connection = connection;
            _accountManagementDbContext = CreateDbContext();

            _supportsTransactions = !ProvidersWithoutTransactionSupport
                .Contains(_accountManagementDbContext.Database.ProviderName);
        }

        internal virtual IAMDbContext CreateDbContext()
        {
            return new(_connection.Configuration);
        }

        public IRepoExtendedGet<AccountEntity> CreateAccountRepo()
        {
            return new EFRepoExtendedGet<AccountEntity>(
                _loggerFactory.CreateLogger<EFRepoExtendedGet<AccountEntity>>(),
                SaveChangesAsync,
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
                SaveChangesAsync,
                _accountManagementDbContext.Users);
        }

        public IRepoExtendedGet<BasicAuthEntity> CreateBasicAuthRepo()
        {
            return new EFRepoExtendedGet<BasicAuthEntity>(
                _loggerFactory.CreateLogger<EFRepoExtendedGet<BasicAuthEntity>>(),
                SaveChangesAsync,
                _accountManagementDbContext.BasicAuths);
        }

        public IUnitOfWorkProvider CreateUnitOfWorkProvider()
        {
            return _supportsTransactions
                ? new EFUoWProvider(_accountManagementDbContext)
                : new MockUoWProvider();
        }

        private async Task SaveChangesAsync() => await _accountManagementDbContext.SaveChangesAsync();
    }
}
