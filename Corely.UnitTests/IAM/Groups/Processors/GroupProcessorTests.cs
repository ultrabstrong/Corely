using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Groups.Entities;
using Corely.IAM.Groups.Models;
using Corely.IAM.Groups.Processors;
using Corely.IAM.Mappers;
using Corely.IAM.Roles.Entities;
using Corely.IAM.Users.Entities;
using Corely.IAM.Validators;
using Corely.UnitTests.ClassData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Groups.Processors;

public class GroupProcessorTests
{
    private const string VALID_GROUP_NAME = "groupname";

    private readonly Fixture _fixture = new();
    private readonly ServiceFactory _serviceFactory = new();
    private readonly GroupProcessor _groupProcessor;

    public GroupProcessorTests()
    {
        _groupProcessor = new GroupProcessor(
            _serviceFactory.GetRequiredService<IRepo<GroupEntity>>(),
            _serviceFactory.GetRequiredService<IReadonlyRepo<AccountEntity>>(),
            _serviceFactory.GetRequiredService<IReadonlyRepo<UserEntity>>(),
            _serviceFactory.GetRequiredService<IReadonlyRepo<RoleEntity>>(),
            _serviceFactory.GetRequiredService<IMapProvider>(),
            _serviceFactory.GetRequiredService<IValidationProvider>(),
            _serviceFactory.GetRequiredService<ILogger<GroupProcessor>>());
    }

    private async Task<int> CreateAccountAsync()
    {
        var accountId = _fixture.Create<int>();
        var account = new AccountEntity { Id = accountId };
        var accountRepo = _serviceFactory.GetRequiredService<IRepo<AccountEntity>>();
        return await accountRepo.CreateAsync(account);
    }

    private async Task<int> CreateUserAsync(params int[] groupIds)
    {
        var userId = _fixture.Create<int>();
        var user = new UserEntity
        {
            Id = userId,
            Groups =
                groupIds
                    ?.Select(g => new GroupEntity { Id = g })
                    ?.ToList()
                ?? []
        };
        var userRepo = _serviceFactory.GetRequiredService<IRepo<UserEntity>>();
        return await userRepo.CreateAsync(user);
    }

    private async Task<int> CreateRoleAsync(params int[] groupIds)
    {
        var roleId = _fixture.Create<int>();
        var role = new RoleEntity
        {
            Id = roleId,
            Groups =
                groupIds
                    ?.Select(g => new GroupEntity { Id = g })
                    ?.ToList()
                ?? []
        };
        var roleRepo = _serviceFactory.GetRequiredService<IRepo<RoleEntity>>();
        return await roleRepo.CreateAsync(role);
    }

    private async Task<int> CreateGroupAsync()
    {
        var accountId = await CreateAccountAsync();
        var group = new GroupEntity
        {
            GroupName = VALID_GROUP_NAME,
            AccountId = accountId
        };
        var groupRepo = _serviceFactory.GetRequiredService<IRepo<GroupEntity>>();
        return await groupRepo.CreateAsync(group);
    }

    [Fact]
    public async Task CreateGroupAsync_Fails_WhenAccountDoesNotExist()
    {
        var request = new CreateGroupRequest(VALID_GROUP_NAME, _fixture.Create<int>());

        var result = await _groupProcessor.CreateGroupAsync(request);

        Assert.Equal(CreateGroupResultCode.AccountNotFoundError, result.ResultCode);
    }

    [Fact]
    public async Task CreateGroupAsync_Fails_WhenGroupExists()
    {
        var request = new CreateGroupRequest(VALID_GROUP_NAME, await CreateAccountAsync());
        await _groupProcessor.CreateGroupAsync(request);

        var result = await _groupProcessor.CreateGroupAsync(request);

        Assert.Equal(CreateGroupResultCode.GroupExistsError, result.ResultCode);
    }

    [Fact]
    public async Task CreateGroupAsync_ReturnsCreateGroupResult()
    {
        var accountId = await CreateAccountAsync();
        var request = new CreateGroupRequest(VALID_GROUP_NAME, accountId);

        var result = await _groupProcessor.CreateGroupAsync(request);

        Assert.Equal(CreateGroupResultCode.Success, result.ResultCode);

        // Verify group is linked to account id
        var groupRepo = _serviceFactory.GetRequiredService<IRepo<GroupEntity>>();
        var groupEntity = await groupRepo.GetAsync(
            g => g.Id == result.CreatedId,
            include: q => q.Include(g => g.Account));
        Assert.NotNull(groupEntity);
        //Assert.NotNull(groupEntity.Account); // Account not available for memory mock repo
        Assert.Equal(accountId, groupEntity.AccountId);
    }

    [Fact]
    public async Task CreateGroupAsync_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _groupProcessor.CreateGroupAsync(null!));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
    public async Task CreateGroupAsync_Throws_WithInvalidGroupName(string groupName)
    {
        var request = new CreateGroupRequest(groupName, await CreateAccountAsync());

        var ex = await Record.ExceptionAsync(() => _groupProcessor.CreateGroupAsync(request));

        Assert.NotNull(ex);
        Assert.IsType<ValidationException>(ex);
    }

    [Fact]
    public async Task AddUsersToGroupAsync_Fails_WhenGroupDoesNotExist()
    {
        var request = new AddUsersToGroupRequest([], _fixture.Create<int>());
        var result = await _groupProcessor.AddUsersToGroupAsync(request);
        Assert.Equal(AddUsersToGroupResultCode.GroupNotFoundError, result.ResultCode);
    }

    [Fact]
    public async Task AddUsersToGroupAsync_Fails_WhenUsersNotProvided()
    {
        var groupId = await CreateGroupAsync();
        var request = new AddUsersToGroupRequest([], groupId);

        var result = await _groupProcessor.AddUsersToGroupAsync(request);

        Assert.Equal(AddUsersToGroupResultCode.InvalidUserIdsError, result.ResultCode);
        Assert.Equal("All user ids not found or already exist in group", result.Message);
    }

    [Fact]
    public async Task AddUsersToGroupAsync_Succeeds_WhenUsersAdded()
    {
        var userId = await CreateUserAsync();
        var groupId = await CreateGroupAsync();
        var request = new AddUsersToGroupRequest([userId], groupId);

        var result = await _groupProcessor.AddUsersToGroupAsync(request);

        Assert.Equal(AddUsersToGroupResultCode.Success, result.ResultCode);

        var groupRepo = _serviceFactory.GetRequiredService<IRepo<GroupEntity>>();
        var groupEntity = await groupRepo.GetAsync(
            g => g.Id == groupId,
            include: q => q.Include(g => g.Users));

        Assert.NotNull(groupEntity);
        Assert.NotNull(groupEntity.Users);
        Assert.Contains(groupEntity.Users, u => u.Id == userId);
    }

    [Fact]
    public async Task AddUsersToGroupAsync_PartiallySucceeds_WhenSomeUsersExistInGroup()
    {
        var groupId = await CreateGroupAsync();
        var existingUserId = await CreateUserAsync(groupId);
        var newUserId = await CreateUserAsync();
        var request = new AddUsersToGroupRequest([existingUserId, newUserId], groupId);

        var result = await _groupProcessor.AddUsersToGroupAsync(request);

        Assert.Equal(AddUsersToGroupResultCode.PartialSuccess, result.ResultCode);
        Assert.Equal("Some user ids not found or already exist in group", result.Message);
        Assert.Equal(1, result.AddedUserCount);
        Assert.NotEmpty(result.InvalidUserIds);
        Assert.Contains(existingUserId, result.InvalidUserIds);
    }

    [Fact]
    public async Task AddUsersToGroupAsync_ReportsInvalidUserIds_WhenUsersDoNotExist()
    {
        var userId = await CreateUserAsync();
        var groupId = await CreateGroupAsync();
        var request = new AddUsersToGroupRequest([userId, -1], groupId);

        var result = await _groupProcessor.AddUsersToGroupAsync(request);

        Assert.Equal(AddUsersToGroupResultCode.PartialSuccess, result.ResultCode);
        Assert.NotEmpty(result.InvalidUserIds);
        Assert.Contains(-1, result.InvalidUserIds);
    }

    [Fact]
    public async Task AddUsersToGroupAsync_Fails_WhenAllUsersAlreadyExistInGroup()
    {
        var groupId = await CreateGroupAsync();
        var userId = await CreateUserAsync(groupId);
        var request = new AddUsersToGroupRequest([userId], groupId);

        var result = await _groupProcessor.AddUsersToGroupAsync(request);

        Assert.Equal(AddUsersToGroupResultCode.InvalidUserIdsError, result.ResultCode);
        Assert.Equal("All user ids not found or already exist in group", result.Message);
        Assert.NotEmpty(result.InvalidUserIds);
        Assert.Contains(userId, result.InvalidUserIds);
    }

    [Fact]
    public async Task AssignRolesToGroupAsync_Fails_WhenGroupDoesNotExist()
    {
        var request = new AssignRolesToGroupRequest([], _fixture.Create<int>());
        var result = await _groupProcessor.AssignRolesToGroupAsync(request);
        Assert.Equal(AssignRolesToGroupResultCode.GroupNotFoundError, result.ResultCode);
    }

    [Fact]
    public async Task AssignRolesToGroupAsync_Fails_WhenRolesNotProvided()
    {
        var groupId = await CreateGroupAsync();
        var request = new AssignRolesToGroupRequest([], groupId);

        var result = await _groupProcessor.AssignRolesToGroupAsync(request);

        Assert.Equal(AssignRolesToGroupResultCode.InvalidRoleIdsError, result.ResultCode);
        Assert.Equal("All role ids not found or already assigned to group", result.Message);
    }

    [Fact]
    public async Task AssignRolesToGroupAsync_Succeeds_WhenRolesAssigned()
    {
        var roleId = await CreateRoleAsync();
        var groupId = await CreateGroupAsync();
        var request = new AssignRolesToGroupRequest([roleId], groupId);

        var result = await _groupProcessor.AssignRolesToGroupAsync(request);

        Assert.Equal(AssignRolesToGroupResultCode.Success, result.ResultCode);

        var groupRepo = _serviceFactory.GetRequiredService<IRepo<GroupEntity>>();
        var groupEntity = await groupRepo.GetAsync(
            g => g.Id == groupId,
            include: q => q.Include(g => g.Roles));

        Assert.NotNull(groupEntity);
        Assert.NotNull(groupEntity.Roles);
        Assert.Contains(groupEntity.Roles, r => r.Id == roleId);
    }

    [Fact]
    public async Task AssignRolesToGroupAsync_PartiallySucceeds_WhenSomeRolesAssignedToGroup()
    {
        var groupId = await CreateGroupAsync();
        var existingRoleId = await CreateRoleAsync(groupId);
        var newRoleId = await CreateRoleAsync();
        var request = new AssignRolesToGroupRequest([existingRoleId, newRoleId], groupId);

        var result = await _groupProcessor.AssignRolesToGroupAsync(request);

        Assert.Equal(AssignRolesToGroupResultCode.PartialSuccess, result.ResultCode);
        Assert.Equal("Some role ids not found or already assigned to group", result.Message);
        Assert.Equal(1, result.AddedRoleCount);
        Assert.NotEmpty(result.InvalidRoleIds);
        Assert.Contains(existingRoleId, result.InvalidRoleIds);
    }

    [Fact]
    public async Task AssignRolesToGroupAsync_ReportsInvalidRoleIds_WhenSomeRolesDoNotExist()
    {
        var roleId = await CreateRoleAsync();
        var groupId = await CreateGroupAsync();
        var request = new AssignRolesToGroupRequest([roleId, -1], groupId);

        var result = await _groupProcessor.AssignRolesToGroupAsync(request);

        Assert.Equal(AssignRolesToGroupResultCode.PartialSuccess, result.ResultCode);
        Assert.NotEmpty(result.InvalidRoleIds);
        Assert.Contains(-1, result.InvalidRoleIds);
    }

    [Fact]
    public async Task AssignRolesToGroupAsync_Fails_WhenAllRolesAlreadyAssignedToGroup()
    {
        var groupId = await CreateGroupAsync();
        var roleId = await CreateRoleAsync(groupId);
        var request = new AssignRolesToGroupRequest([roleId], groupId);
        await _groupProcessor.AssignRolesToGroupAsync(request);

        var result = await _groupProcessor.AssignRolesToGroupAsync(request);

        Assert.Equal(AssignRolesToGroupResultCode.InvalidRoleIdsError, result.ResultCode);
        Assert.Equal("All role ids not found or already assigned to group", result.Message);
        Assert.NotEmpty(result.InvalidRoleIds);
        Assert.Contains(roleId, result.InvalidRoleIds);
    }

}
