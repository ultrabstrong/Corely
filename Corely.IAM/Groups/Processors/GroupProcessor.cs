using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Exceptions;
using Corely.IAM.Groups.Entities;
using Corely.IAM.Groups.Exceptions;
using Corely.IAM.Groups.Models;
using Corely.IAM.Mappers;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Groups.Processors
{
    internal class GroupProcessor : ServiceBase, IGroupProcessor
    {
        private readonly IRepo<GroupEntity> _groupRepo;
        private readonly IReadonlyRepo<AccountEntity> _accountRepo;

        public GroupProcessor(
            IRepo<GroupEntity> groupRepo,
            IReadonlyRepo<AccountEntity> accountRepo,
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger<GroupProcessor> logger)
            : base(mapProvider, validationProvider, logger)
        {
            _groupRepo = groupRepo.ThrowIfNull(nameof(groupRepo));
            _accountRepo = accountRepo.ThrowIfNull(nameof(accountRepo));
        }

        public async Task<CreateResult> CreateGroupAsync(CreateGroupRequest createGroupRequest)
        {
            var group = MapThenValidateTo<Group>(createGroupRequest);

            Logger.LogInformation("Creating group {GroupName}", createGroupRequest.GroupName);

            await ThrowIfGroupExists(group.AccountId, group.GroupName);

            var accountEntity = await _accountRepo.GetAsync(group.AccountId);
            if (accountEntity == null)
            {
                Logger.LogWarning("Account with Id {AccountId} not found", group.AccountId);
                throw new AccountDoesNotExistException($"Account with Id {group.AccountId} not found");
            }

            var groupEntity = MapTo<GroupEntity>(group);
            var createdId = await _groupRepo.CreateAsync(groupEntity);

            Logger.LogInformation("Group {GroupName} created with Id {GroupId}", group.GroupName, createdId);
            return new CreateResult(true, string.Empty, createdId);
        }

        private async Task ThrowIfGroupExists(int accountId, string groupName)
        {
            if (await _groupRepo.AnyAsync(g =>
                g.AccountId == accountId && g.GroupName == groupName))
            {
                Logger.LogWarning("Group with name {GroupName} already exists", groupName);
                throw new GroupExistsException($"Group with name {groupName} already exists");
            }
        }
    }
}
