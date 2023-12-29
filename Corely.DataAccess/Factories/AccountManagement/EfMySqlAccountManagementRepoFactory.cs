using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.Auth;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Factories.AccountManagement
{
    internal class EfMySqlAccountManagementRepoFactory(
        ILoggerFactory loggerFactory,
        string connection)
        : IAccountManagementRepoFactory
    {
        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly string _connection = connection;

        private AccountManagementDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<AccountManagementDbContext> optionsBuilder = new();
            optionsBuilder.UseMySql(_connection, ServerVersion.AutoDetect(_connection));
            return new(optionsBuilder.Options);
        }

        public IAuthRepo<BasicAuthEntity> GetBasicAuthRepo()
        {
            return new EFBasicAuthRepo(_loggerFactory.CreateLogger<EFBasicAuthRepo>(), CreateDbContext());
        }

        public IUserRepo GetUserRepo()
        {
            return new EFUserRepo(_loggerFactory.CreateLogger<EFUserRepo>(), CreateDbContext());
        }
    }
}
