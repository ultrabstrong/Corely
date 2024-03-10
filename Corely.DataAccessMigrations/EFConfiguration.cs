using Corely.DataAccess.Connections;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Corely.DataAccessMigrations
{
    internal class EFConfiguration(string connectionString) : MySqlEFConfigurationBase(connectionString)
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
