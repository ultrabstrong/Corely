﻿using Corely.Common.Extensions;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Exceptions;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Mappers;
using Corely.IAM.Models;
using Corely.IAM.Repos;
using Corely.IAM.Security.Services;
using Corely.IAM.Services;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Accounts.Services
{
    internal class AccountService : ServiceBase, IAccountService
    {
        private readonly IRepoExtendedGet<AccountEntity> _accountRepo;
        private readonly ISecurityService _securityService;

        public AccountService(
            IRepoExtendedGet<AccountEntity> accountRepo,
            ISecurityService securityService,
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger<AccountService> logger)
            : base(mapProvider, validationProvider, logger)
        {
            _accountRepo = accountRepo.ThrowIfNull(nameof(accountRepo));
            _securityService = securityService.ThrowIfNull(nameof(securityService));
        }

        public async Task<CreateResult> CreateAccountAsync(CreateAccountRequest request)
        {
            var account = MapThenValidateTo<Account>(request);

            Logger.LogInformation("Creating account {Account}", request.AccountName);

            await ThrowIfAccountExists(account.AccountName);

            account.SymmetricKey = _securityService.GetSymmetricKeyEncryptedWithSystemKey();
            var accountEntity = MapTo<AccountEntity>(account);
            var createdId = await _accountRepo.CreateAsync(accountEntity);

            Logger.LogInformation("Account {Account} created with Id {Id}", account.AccountName, createdId);
            return new CreateResult(true, string.Empty, createdId);
        }

        private async Task ThrowIfAccountExists(string accountName)
        {
            var existingAccount = await _accountRepo.GetAsync(a => a.AccountName == accountName);
            if (existingAccount != null)
            {
                Logger.LogWarning("Account {Account} already exists", accountName);
                throw new AccountExistsException($"Account {accountName} already exists");
            }
        }
    }
}
