using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Groups.Entities;
using Corely.IAM.Groups.Enums;
using Corely.IAM.Groups.Models;
using Corely.IAM.Mappers;
using Corely.IAM.Processors;
using Corely.IAM.Users.Entities;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Groups.Processors;

internal class GroupProcessor : ProcessorBase, IGroupProcessor
{
    private readonly IRepo<GroupEntity> _groupRepo;
    private readonly IReadonlyRepo<AccountEntity> _accountRepo;
    private readonly IReadonlyRepo<UserEntity> _userRepo;

    public GroupProcessor(
        IRepo<GroupEntity> groupRepo,
        IReadonlyRepo<AccountEntity> accountRepo,
        IReadonlyRepo<UserEntity> userRepo,
        IMapProvider mapProvider,
        IValidationProvider validationProvider,
        ILogger<GroupProcessor> logger)
        : base(mapProvider, validationProvider, logger)
    {
        _groupRepo = groupRepo.ThrowIfNull(nameof(groupRepo));
        _accountRepo = accountRepo.ThrowIfNull(nameof(accountRepo));
        _userRepo = userRepo.ThrowIfNull(nameof(userRepo));
    }

    public async Task<CreateGroupResult> CreateGroupAsync(CreateGroupRequest createGroupRequest)
    {
        return await LogRequestResultAspect(nameof(GroupProcessor), nameof(CreateGroupAsync), createGroupRequest, async () =>
        {
            var group = MapThenValidateTo<Group>(createGroupRequest);

            if (await _groupRepo.AnyAsync(g =>
            g.AccountId == group.AccountId && g.GroupName == group.GroupName))
            {
                Logger.LogWarning("Group with name {GroupName} already exists", group.GroupName);
                return new CreateGroupResult(CreateGroupResultCode.GroupExistsError, $"Group with name {group.GroupName} already exists", -1);
            }

            var accountEntity = await _accountRepo.GetAsync(group.AccountId);
            if (accountEntity == null)
            {
                Logger.LogWarning("Account with Id {AccountId} not found", group.AccountId);
                return new CreateGroupResult(CreateGroupResultCode.AccountNotFoundError, $"Account with Id {group.AccountId} not found", -1);
            }

            var groupEntity = MapTo<GroupEntity>(group)!; // group is validated
            var createdId = await _groupRepo.CreateAsync(groupEntity);

            return new CreateGroupResult(CreateGroupResultCode.Success, string.Empty, createdId);
        });
    }

    public async Task<AddUsersToGroupResult> AddUsersToGroupAsync(AddUsersToGroupRequest request)
    {
        return await LogRequestResultAspect(nameof(GroupProcessor), nameof(AddUsersToGroupAsync), request, async () =>
        {
            var groupEntity = await _groupRepo.GetAsync(g => g.Id == request.GroupId);

            if (groupEntity == null)
            {
                Logger.LogWarning("Group with Id {GroupId} not found", request.GroupId);
                return new AddUsersToGroupResult(AddUsersToGroupResultCode.GroupNotFoundError,
                    $"Group with Id {request.GroupId} not found", 0, request.UserIds);
            }

            var userEntities = await _userRepo.ListAsync(
                u => request.UserIds.Contains(u.Id)
                && !u.Groups!.Any(g => g.Id == groupEntity.Id));

            if (userEntities.Count == 0)
            {
                Logger.LogInformation("All user ids not found or already exist in group : {@InvalidUserIds}", request.UserIds);
                return new AddUsersToGroupResult(AddUsersToGroupResultCode.InvalidUserIdsError,
                    "All user ids not found or already exist in group", 0, request.UserIds);
            }

            groupEntity.Users ??= [];
            foreach (var user in userEntities)
            {
                groupEntity.Users.Add(user);
            }

            await _groupRepo.UpdateAsync(groupEntity);

            var invalidUserIds = request.UserIds.Except(userEntities.Select(u => u.Id)).ToList();

            if (invalidUserIds.Count > 0)
            {
                Logger.LogInformation("Some user ids not found or already exist in group : {@InvalidUserIds}", invalidUserIds);
                return new AddUsersToGroupResult(AddUsersToGroupResultCode.PartialSuccess,
                    "Some user ids not found or already exist in group", userEntities.Count, invalidUserIds);
            }

            return new AddUsersToGroupResult(AddUsersToGroupResultCode.Success, string.Empty, userEntities.Count, invalidUserIds);
        });
    }
}