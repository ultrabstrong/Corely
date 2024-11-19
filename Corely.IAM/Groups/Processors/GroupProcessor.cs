﻿using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Exceptions;
using Corely.IAM.Groups.Entities;
using Corely.IAM.Groups.Exceptions;
using Corely.IAM.Groups.Models;
using Corely.IAM.Mappers;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Corely.IAM.Users.Entities;
using Corely.IAM.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Groups.Processors
{
    internal class GroupProcessor : ServiceBase, IGroupProcessor
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

        public async Task<CreateResult> CreateGroupAsync(CreateGroupRequest createGroupRequest)
        {
            var group = MapThenValidateTo<Group>(createGroupRequest);

            Logger.LogDebug("Creating group {GroupName}", createGroupRequest.GroupName);

            await ThrowIfGroupCannotBeAdded(group.AccountId, group.GroupName);

            var groupEntity = MapTo<GroupEntity>(group);
            var createdId = await _groupRepo.CreateAsync(groupEntity);

            Logger.LogDebug("Group {GroupName} created with Id {GroupId}", group.GroupName, createdId);
            return new CreateResult(true, string.Empty, createdId);
        }

        private async Task ThrowIfGroupCannotBeAdded(int accountId, string groupName)
        {
            if (await _groupRepo.AnyAsync(g =>
                g.AccountId == accountId && g.GroupName == groupName))
            {
                Logger.LogWarning("Group with name {GroupName} already exists", groupName);
                throw new GroupExistsException($"Group with name {groupName} already exists");
            }

            var accountEntity = await _accountRepo.GetAsync(accountId);
            if (accountEntity == null)
            {
                Logger.LogWarning("Account with Id {AccountId} not found", accountId);
                throw new AccountDoesNotExistException($"Account with Id {accountId} not found");
            }
        }

        public async Task<AddUsersToGroupResult> AddUsersToGroupAsync(AddUsersToGroupRequest addUsersToGroupRequest)
        {
            Logger.LogDebug("Adding user ids {@UserIds} to group id {GroupId}", addUsersToGroupRequest.UserIds, addUsersToGroupRequest.GroupId);

            var groupEntity = await GetGroupOrThrowIfNotFound(addUsersToGroupRequest.GroupId);
            var userEntities = await _userRepo.ListAsync(u => addUsersToGroupRequest.UserIds.Contains(u.Id));

            if (userEntities.Count == 0)
            {
                Logger.LogInformation("No users found for user ids {@UserIds}", addUsersToGroupRequest.UserIds);
                return new AddUsersToGroupResult(false, "No users found for provided user ids", 0);
            }

            groupEntity.Users ??= [];
            foreach (var user in userEntities)
            {
                groupEntity.Users.Add(user);
            }

            await _groupRepo.UpdateAsync(groupEntity);

            var invalidUserIds = addUsersToGroupRequest.UserIds.Except(userEntities.Select(u => u.Id)).ToList();
            if (invalidUserIds.Count > 0)
            {
                Logger.LogInformation("Some users were not found. Invalid user ids : {@InvalidUserIds}", invalidUserIds);
            }

            Logger.LogDebug("Added {UserCount} users to group {GroupId}", userEntities.Count, addUsersToGroupRequest.GroupId);

            return new AddUsersToGroupResult(true, string.Empty, userEntities.Count, invalidUserIds);
        }

        private async Task<GroupEntity> GetGroupOrThrowIfNotFound(int groupId)
        {
            var groupEntity = await _groupRepo.GetAsync(
                g => g.Id == groupId,
                include: q => q.Include(g => g.Users));

            if (groupEntity == null)
            {
                Logger.LogWarning("Group with Id {GroupId} not found", groupId);
                throw new GroupDoesNotExistException($"Group with Id {groupId} not found");
            }

            return groupEntity;
        }
    }
}
