﻿using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Exceptions;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Mappers;
using Corely.IAM.Models;
using Corely.IAM.Processors;
using Corely.IAM.Security.Processors;
using Corely.IAM.Users.Entities;
using Corely.IAM.Users.Exceptions;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Accounts.Processors;

internal class AccountProcessor : ProcessorBase, IAccountProcessor
{
    private readonly IRepo<AccountEntity> _accountRepo;
    private readonly IReadonlyRepo<UserEntity> _userRepo;
    private readonly ISecurityProcessor _securityService;

    public AccountProcessor(
        IRepo<AccountEntity> accountRepo,
        IReadonlyRepo<UserEntity> userRepo,
        ISecurityProcessor securityService,
        IMapProvider mapProvider,
        IValidationProvider validationProvider,
        ILogger<AccountProcessor> logger)
        : base(mapProvider, validationProvider, logger)
    {
        _accountRepo = accountRepo.ThrowIfNull(nameof(accountRepo));
        _userRepo = userRepo.ThrowIfNull(nameof(userRepo));
        _securityService = securityService.ThrowIfNull(nameof(securityService));
    }

    public async Task<CreateResult> CreateAccountAsync(CreateAccountRequest request)
    {
        return await LogRequestResultAspect(nameof(AccountProcessor), nameof(CreateAccountAsync), request, async () =>
        {
            var account = MapThenValidateTo<Account>(request);

            await ThrowIfAccountExists(account.AccountName);
            var userEntity = await GetUserOrThrowIfNotFound(request.OwnerUserId);

            account.SymmetricKeys = [_securityService.GetSymmetricEncryptionKeyEncryptedWithSystemKey()];
            account.AsymmetricKeys = [
                _securityService.GetAsymmetricEncryptionKeyEncryptedWithSystemKey(),
                _securityService.GetAsymmetricSignatureKeyEncryptedWithSystemKey()];

            var accountEntity = MapTo<AccountEntity>(account)!; // account is validated
            accountEntity.Users = [userEntity];
            var createdId = await _accountRepo.CreateAsync(accountEntity);

            return new CreateResult(true, string.Empty, createdId);
        });
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

    private async Task<UserEntity> GetUserOrThrowIfNotFound(int userId)
    {
        var userEntity = await _userRepo.GetAsync(userId);
        if (userEntity == null)
        {
            Logger.LogWarning("User with Id {UserId} not found", userId);
            throw new UserDoesNotExistException($"User with Id {userId} not found");
        }
        return userEntity;
    }

    public async Task<Account?> GetAccountAsync(int accountId)
    {
        return await LogRequestAspect(nameof(AccountProcessor), nameof(GetAccountAsync), accountId, async () =>
        {
            var accountEntity = await _accountRepo.GetAsync(accountId);
            var account = MapTo<Account>(accountEntity);

            if (account == null)
            {
                Logger.LogInformation("Account with Id {AccountId} not found", accountId);
            }
            return account;
        });
    }

    public async Task<Account?> GetAccountAsync(string accountName)
    {
        return await LogRequestResultAspect(nameof(AccountProcessor), nameof(GetAccountAsync), accountName, async () =>
        {
            var accountEntity = await _accountRepo.GetAsync(a => a.AccountName == accountName);
            var account = MapTo<Account>(accountEntity);

            if (account == null)
            {
                Logger.LogInformation("Account with name {AccountName} not found", accountName);
            }

            return account;
        });
    }
}
