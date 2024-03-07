using Corely.Common.Extensions;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Repos;

namespace Corely.Domain.Services.Accounts
{
    internal class AccountEntityGetterService : IEntityGetterService<AccountEntity>
    {
        private readonly IRepoExtendedGet<AccountEntity> _accountRepo;

        public AccountEntityGetterService(IRepoExtendedGet<AccountEntity> accountRepo)
        {
            _accountRepo = accountRepo.ThrowIfNull(nameof(accountRepo));
        }

        public async Task<AccountEntity?> GetAsync(int id)
        {
            return await _accountRepo.GetAsync(id);
        }
    }
}
