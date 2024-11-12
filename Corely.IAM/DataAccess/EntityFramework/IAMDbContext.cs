using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Auth.Entities;
using Corely.IAM.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Corely.IAM.DataAccess.EntityFramework
{
    internal class IamDbContext : DbContext
    {
        private readonly IEFConfiguration _configuration;

        public IamDbContext(IEFConfiguration configuration)
            : base()
        {
            _configuration = configuration;
        }

        public DbSet<AccountEntity> Accounts { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<BasicAuthEntity> BasicAuths { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _configuration.Configure(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var configurationType = typeof(EntityConfigurationBase<>);
            var configurations = GetType().Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null
                            && t.BaseType.IsGenericType
                            && t.BaseType.GetGenericTypeDefinition() == configurationType);

            foreach (var config in configurations)
            {
                var instance = Activator.CreateInstance(config, _configuration.GetDbTypes());
                modelBuilder.ApplyConfiguration((dynamic)instance!);
            }
        }
    }
}
