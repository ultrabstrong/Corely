using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.Auth;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Auth;
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

        private AccountManagementDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<AccountManagementDbContext> optionsBuilder = new();
            optionsBuilder.UseMySql(_connection, ServerVersion.AutoDetect(_connection));
            return new(optionsBuilder.Options);
        }

        public IAuthRepo<BasicAuthEntity> CreateBasicAuthRepo()
        {
            return new EFBasicAuthRepo(_loggerFactory.CreateLogger<EFBasicAuthRepo>(), CreateDbContext());
        }

        public IUserRepo CreateUserRepo()
        {
            return new EFUserRepo(_loggerFactory.CreateLogger<EFUserRepo>(), CreateDbContext());
        }
    }
}
