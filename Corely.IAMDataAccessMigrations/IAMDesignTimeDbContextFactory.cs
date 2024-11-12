using Corely.IAM.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Corely.IAMDataAccessMigrations
{
    internal class IAMDesignTimeDbContextFactory : IDesignTimeDbContextFactory<IamDbContext>
    {

        public IamDbContext CreateDbContext(string[] args)
        {
            var configuration = new EFMySqlConfiguration(ConfigurationProvider.GetConnectionString());
            var optionsBuilder = new DbContextOptionsBuilder<IamDbContext>();
            configuration.Configure(optionsBuilder);
            return new IamDbContext(configuration);
        }
    }
}
