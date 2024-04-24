using Corely.DataAccess.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Corely.DataAccessMigrations
{
    internal class EFConfiguration(string connectionString) : EFMySqlConfigurationBase(connectionString)
    {
        public override void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
        }
    }
}
