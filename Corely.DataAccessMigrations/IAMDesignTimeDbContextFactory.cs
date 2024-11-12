﻿using Corely.IAM.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Corely.DataAccessMigrations
{
    internal class IAMDesignTimeDbContextFactory : IDesignTimeDbContextFactory<IAMDbContext>
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
