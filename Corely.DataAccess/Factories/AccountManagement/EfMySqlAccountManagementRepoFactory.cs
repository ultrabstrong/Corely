using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.Auth;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Factories.AccountManagement
{
    internal class EfMySqlAccountManagementRepoFactory : IAccountManagementRepoFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly string _connection;

        public EfMySqlAccountManagementRepoFactory(
            ILoggerFactory loggerFactory,
            string connection)
        {
            _loggerFactory = loggerFactory;
            _connection = connection;
        }

        internal virtual AccountManagementDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<AccountManagementDbContext> optionsBuilder = new();
            optionsBuilder.UseMySql(_connection, GetServerVersion());
            return new(optionsBuilder.Options);
        }

        internal virtual ServerVersion GetServerVersion()
        {
            return ServerVersion.AutoDetect(_connection);
        }

        public IRepoExtendedGet<BasicAuthEntity> CreateBasicAuthRepo()
        {
            return new EFBasicAuthRepo(_loggerFactory.CreateLogger<EFBasicAuthRepo>(), CreateDbContext());
        }

        public IRepoExtendedGet<UserEntity> CreateUserRepo()
        {
            return new EFUserRepo(_loggerFactory.CreateLogger<EFUserRepo>(), CreateDbContext());
        }
    }
}
