using Corely.DataAccess.EntityFramework.IAM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Corely.DataAccessMigrations
{
    public class IAMDesignTimeDbContextFactory : IDesignTimeDbContextFactory<IAMDbContext>
    {

        public IAMDbContext CreateDbContext(string[] args)
        {
            var configuration = new EFMySqlConfiguration(ConfigurationProvider.GetConnectionString());
            var optionsBuilder = new DbContextOptionsBuilder<IAMDbContext>();
            configuration.Configure(optionsBuilder);
            return new IAMDbContext(configuration);
        }
    }
}
