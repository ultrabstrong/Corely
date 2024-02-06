﻿using Corely.DataAccess.Connections;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.Accounts;
using Corely.DataAccess.Repos.Auth;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Factories.AccountManagement
{
    internal class EFAccountManagementRepoFactory : IAccountManagementRepoFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IEFConfiguration _configuration;

        public EFAccountManagementRepoFactory(
            ILoggerFactory loggerFactory,
            IEFConfiguration configuration)
        {
            _loggerFactory = loggerFactory;
            _configuration = configuration;
        }

        internal virtual AccountManagementDbContext CreateDbContext()
        {
            return new(_configuration);
        }

        public IRepoExtendedGet<AccountEntity> CreateAccountRepo()
        {
            return new EFAccountRepo(_loggerFactory.CreateLogger<EFAccountRepo>(), CreateDbContext());
        }

        public IRepoExtendedGet<UserEntity> CreateUserRepo()
        {
            return new EFUserRepo(_loggerFactory.CreateLogger<EFUserRepo>(), CreateDbContext());
        }
        public IRepoExtendedGet<BasicAuthEntity> CreateBasicAuthRepo()
        {
            return new EFBasicAuthRepo(_loggerFactory.CreateLogger<EFBasicAuthRepo>(), CreateDbContext());
        }
    }
}