using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.Auth;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;
using Microsoft.EntityFrameworkCore;
using Serilog
;

namespace Corely.DataAccess.Factories.AccountManagement
{
    internal class EfMySqlAccountManagementRepoFactory(
        ILogger logger, string connection)
        : IAccountManagementRepoFactory
    {
        private readonly ILogger _logger = logger;
        private readonly string _connection = connection;

        private AccountManagementDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<AccountManagementDbContext> optionsBuilder = new();
            optionsBuilder.UseMySql(_connection, ServerVersion.AutoDetect(_connection));
            return new(optionsBuilder.Options);
        }

        public IAuthRepo<BasicAuthEntity> GetBasicAuthRepo()
        {
            return new EFBasicAuthRepo(_logger, CreateDbContext());
        }

        public IUserRepo GetUserRepo()
        {
            return new EFUserRepo(_logger, CreateDbContext());
        }
    }
}
