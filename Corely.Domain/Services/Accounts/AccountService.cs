using Corely.Common.Extensions;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Exceptions;
using Corely.Domain.Mappers;
using Corely.Domain.Models;
using Corely.Domain.Models.Accounts;
using Corely.Domain.Repos;
using Corely.Domain.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.Domain.Services.Accounts
{
    public class AccountService : ServiceBase, IAccountService
    {
        private readonly IRepoExtendedGet<AccountEntity> _accountRepo;

        public AccountService(
            IRepoExtendedGet<AccountEntity> accountRepo,
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger<AccountService> logger)
            : base(mapProvider, validationProvider, logger)
        {
            _accountRepo = accountRepo.ThrowIfNull(nameof(accountRepo));
        }

        public async Task<CreateResult> CreateAccountAsync(CreateAccountRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            logger.LogInformation("Creating account {Account}", request.AccountName);

            var account = MapToValid<Account>(request);
            await ThrowIfAccountExists(account.AccountName);

            var accountEntity = mapProvider.Map<AccountEntity>(account);
            var createdId = await _accountRepo.CreateAsync(accountEntity);

            logger.LogInformation("Account {Account} created with Id {Id}", account.AccountName, createdId);
            return new CreateResult(true, "", createdId);
        }

        private async Task ThrowIfAccountExists(string accountName)
        {
            var existingAccount = await _accountRepo.GetAsync(a => a.AccountName == accountName);
            if (existingAccount != null)
            {
                logger.LogWarning("Account {Account} already exists", accountName);
                throw new AccountExistsException(accountName);
            }
        }
    }
}
