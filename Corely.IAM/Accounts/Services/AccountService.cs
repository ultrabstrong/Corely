using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Exceptions;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Mappers;
using Corely.IAM.Models;
using Corely.IAM.Security.Services;
using Corely.IAM.Services;
using Corely.IAM.Users.Entities;
using Corely.IAM.Users.Exceptions;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Accounts.Services
{
    internal class AccountService : ServiceBase, IAccountService
    {
        private readonly IRepoExtendedGet<AccountEntity> _accountRepo;
        private readonly IReadonlyRepo<UserEntity> _userRepo;
        private readonly ISecurityService _securityService;

        public AccountService(
            IRepoExtendedGet<AccountEntity> accountRepo,
            IReadonlyRepo<UserEntity> userRepo,
            ISecurityService securityService,
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger<AccountService> logger)
            : base(mapProvider, validationProvider, logger)
        {
            _accountRepo = accountRepo.ThrowIfNull(nameof(accountRepo));
            _userRepo = userRepo.ThrowIfNull(nameof(userRepo));
            _securityService = securityService.ThrowIfNull(nameof(securityService));
        }

        public async Task<CreateResult> CreateAccountAsync(CreateAccountRequest request)
        {
            var account = MapThenValidateTo<Account>(request);

            Logger.LogInformation("Creating account {Account}", request.AccountName);

            await ThrowIfAccountExists(account.AccountName);

            var userEntity = await _userRepo.GetAsync(request.UserIdOfOwner);
            if (userEntity == null)
            {
                Logger.LogWarning("User with Id {UserId} not found", request.UserIdOfOwner);
                throw new UserDoesNotExistException($"User with Id {request.UserIdOfOwner} not found");
            }

            account.SymmetricKeys = [_securityService.GetSymmetricEncryptionKeyEncryptedWithSystemKey()];
            account.AsymmetricKeys = [
                _securityService.GetAsymmetricEncryptionKeyEncryptedWithSystemKey(),
                _securityService.GetAsymmetricSignatureKeyEncryptedWithSystemKey()];

            var accountEntity = MapTo<AccountEntity>(account);
            accountEntity.Users = [userEntity];
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

        public async Task<Account?> GetAccountAsync(int accountId)
        {
            var accountEntity = await _accountRepo.GetAsync(accountId);

            if (accountEntity == null)
            {
                Logger.LogWarning("Account with Id {AccountId} not found", accountId);
                return null;
            }

            return MapTo<Account>(accountEntity);
        }

        public async Task<Account?> GetAccountAsync(string accountName)
        {
            var accountEntity = await _accountRepo.GetAsync(a => a.AccountName == accountName);

            if (accountEntity == null)
            {
                Logger.LogWarning("Account with Name {AccountName} not found", accountName);
                return null;
            }

            return MapTo<Account>(accountEntity);
        }
    }
}
