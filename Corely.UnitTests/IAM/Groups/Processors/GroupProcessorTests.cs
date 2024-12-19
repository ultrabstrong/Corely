using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Exceptions;
using Corely.IAM.Groups.Entities;
using Corely.IAM.Groups.Enums;
using Corely.IAM.Groups.Exceptions;
using Corely.IAM.Groups.Models;
using Corely.IAM.Groups.Processors;
using Corely.IAM.Mappers;
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

    [Fact]
    public async Task CreateGroupAsync_Throws_WhenAccountDoesNotExist()
    {
        var createGroupRequest = new CreateGroupRequest(VALID_GROUP_NAME, _fixture.Create<int>());

        var ex = await Record.ExceptionAsync(() => _groupProcessor.CreateGroupAsync(createGroupRequest));

        Assert.NotNull(ex);
        Assert.IsType<AccountDoesNotExistException>(ex);
    }

    [Fact]
    public async Task CreateGroupAsync_Throws_WhenGroupExists()
    {
        var createGroupRequest = new CreateGroupRequest(VALID_GROUP_NAME, await CreateAccountAsync());
        await _groupProcessor.CreateGroupAsync(createGroupRequest);

        var ex = await Record.ExceptionAsync(() => _groupProcessor.CreateGroupAsync(createGroupRequest));

        Assert.NotNull(ex);
        Assert.IsType<GroupExistsException>(ex);
    }

    [Fact]
    public async Task CreateGroupAsync_ReturnsCreateGroupResult()
    {
        var accountId = await CreateAccountAsync();
        var createGroupRequest = new CreateGroupRequest(VALID_GROUP_NAME, accountId);

        var createGroupResult = await _groupProcessor.CreateGroupAsync(createGroupRequest);

        Assert.True(createGroupResult.IsSuccess);

        // Verify group is linked to account id
        var groupRepo = _serviceFactory.GetRequiredService<IRepo<GroupEntity>>();
        var groupEntity = await groupRepo.GetAsync(
            g => g.Id == createGroupResult.CreatedId,
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

    [Theory, ClassData(typeof(EmptyAndWhitespace))]
    public async Task CreateGroupAsync_Throws_WithInvalidGroupName(string groupName)
    {
        var createGroupRequest = new CreateGroupRequest(groupName, await CreateAccountAsync());

        var ex = await Record.ExceptionAsync(() => _groupProcessor.CreateGroupAsync(createGroupRequest));

        Assert.NotNull(ex);
        Assert.IsType<ValidationException>(ex);
    }

    [Fact]
    public async Task AddUsersToGroupAsync_Throws_WhenGroupDoesNotExist()
    {
        var addUsersToGroupRequest = new AddUsersToGroupRequest([], _fixture.Create<int>());

        var addUsersToGroupResult = await _groupProcessor.AddUsersToGroupAsync(addUsersToGroupRequest);

        var ex = await Record.ExceptionAsync(() => _groupProcessor.AddUsersToGroupAsync(addUsersToGroupRequest));
    }

    [Fact]
    public async Task AddUsersToGroupAsync_ReturnsFailure_WhenUsersNotProvided()
    {
        var accountId = await CreateAccountAsync();
        var createGroupRequest = new CreateGroupRequest(VALID_GROUP_NAME, accountId);
        var createGroupResult = await _groupProcessor.CreateGroupAsync(createGroupRequest);

        var addUsersToGroupRequest = new AddUsersToGroupRequest([], createGroupResult.CreatedId);

        var addUsersToGroupResult = await _groupProcessor.AddUsersToGroupAsync(addUsersToGroupRequest);

        Assert.Equal(AddUsersToGroupResultCode.InvalidUserIdsError, addUsersToGroupResult.ResultCode);
        Assert.Equal("All user ids not found or already exist in group", addUsersToGroupResult.Message);
    }

    [Fact]
    public async Task AddUsersToGroupAsync_ReturnsSuccess_WhenUsersAdded()
    {
        var userId = await CreateUserAsync();
        var accountId = await CreateAccountAsync();
        var createGroupRequest = new CreateGroupRequest(VALID_GROUP_NAME, accountId);
        var createGroupResult = await _groupProcessor.CreateGroupAsync(createGroupRequest);
        var addUsersToGroupRequest = new AddUsersToGroupRequest([userId], createGroupResult.CreatedId);

        var addUsersToGroupResult = await _groupProcessor.AddUsersToGroupAsync(addUsersToGroupRequest);

        Assert.Equal(AddUsersToGroupResultCode.Success, addUsersToGroupResult.ResultCode);

        var groupRepo = _serviceFactory.GetRequiredService<IRepo<GroupEntity>>();

        var groupEntity = await groupRepo.GetAsync(
            g => g.Id == createGroupResult.CreatedId,
            include: q => q.Include(g => g.Users));

        Assert.NotNull(groupEntity);
        Assert.NotNull(groupEntity.Users);
        Assert.Contains(groupEntity.Users, u => u.Id == userId);
    }

    [Fact]
    public async Task AddUsersToGroupAsync_ReportsInvalidUserIds_WhenUsersDoNotExist()
    {
        var userId = await CreateUserAsync();
        var accountId = await CreateAccountAsync();
        var createGroupRequest = new CreateGroupRequest(VALID_GROUP_NAME, accountId);
        var createGroupResult = await _groupProcessor.CreateGroupAsync(createGroupRequest);
        var addUsersToGroupRequest = new AddUsersToGroupRequest([userId, -1], createGroupResult.CreatedId);

        var addUsersToGroupResult = await _groupProcessor.AddUsersToGroupAsync(addUsersToGroupRequest);

        Assert.Equal(AddUsersToGroupResultCode.PartialSuccess, addUsersToGroupResult.ResultCode);
        Assert.NotEmpty(addUsersToGroupResult.InvalidUserIds);
        Assert.Contains(-1, addUsersToGroupResult.InvalidUserIds);
    }

    [Fact]
    public async Task AddUsersToGroupAsync_ReturnsFailure_WhenAllUsersAlreadyExistInGroup()
    {
        var accountId = await CreateAccountAsync();
        var createGroupRequest = new CreateGroupRequest(VALID_GROUP_NAME, accountId);
        var createGroupResult = await _groupProcessor.CreateGroupAsync(createGroupRequest);
        var userId = await CreateUserAsync(createGroupResult.CreatedId);
        var addUsersToGroupRequest = new AddUsersToGroupRequest([userId], createGroupResult.CreatedId);

        var addUsersToGroupResult = await _groupProcessor.AddUsersToGroupAsync(addUsersToGroupRequest);

        Assert.Equal(AddUsersToGroupResultCode.InvalidUserIdsError, addUsersToGroupResult.ResultCode);
        Assert.Equal("All user ids not found or already exist in group", addUsersToGroupResult.Message);
        Assert.NotEmpty(addUsersToGroupResult.InvalidUserIds);
        Assert.Contains(userId, addUsersToGroupResult.InvalidUserIds);
    }
}
