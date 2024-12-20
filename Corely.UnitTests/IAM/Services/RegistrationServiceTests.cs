﻿using AutoFixture;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Accounts.Processors;
using Corely.IAM.BasicAuths.Models;
using Corely.IAM.BasicAuths.Processors;
using Corely.IAM.Enums;
using Corely.IAM.Groups.Models;
using Corely.IAM.Groups.Processors;
using Corely.IAM.Models;
using Corely.IAM.Roles.Models;
using Corely.IAM.Roles.Processors;
using Corely.IAM.Services;
using Corely.IAM.Users.Models;
using Corely.IAM.Users.Processors;
using Corely.UnitTests.IAM.Processors;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Services;

public class RegistrationServiceTests : ProcessorBaseTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IUnitOfWorkProvider> _unitOfWorkProviderMock = new();
    private readonly Mock<IAccountProcessor> _accountProcessorMock;
    private readonly Mock<IUserProcessor> _userProcessorMock;
    private readonly Mock<IBasicAuthProcessor> _basicAuthProcessorMock;
    private readonly Mock<IGroupProcessor> _groupProcessorMock;
    private readonly Mock<IRoleProcessor> _roleProcessorMock;
    private readonly RegistrationService _registrationService;

    private CreateAccountResultCode _createAccountResultCode = CreateAccountResultCode.Success;
    private CreateUserResultCode _createUserResultCode = CreateUserResultCode.Success;
    private UpsertBasicAuthResultCode _upsertBasicAuthResultCode = UpsertBasicAuthResultCode.Success;
    private CreateGroupResultCode _createGroupResultCode = CreateGroupResultCode.Success;
    private CreateRoleResultCode _createRoleResultCode = CreateRoleResultCode.Success;

    private AddUsersToGroupResult _addUsersToGroupResult = new(AddUsersToGroupResultCode.Success, string.Empty, 0);
    private AssignRolesToGroupResult _assignRolesToGroupResult = new(AssignRolesToGroupResultCode.Success, string.Empty, 0);

    public RegistrationServiceTests() : base()
    {
        _accountProcessorMock = GetMockAccountProcessor();
        _userProcessorMock = GetMockUserProcessor();
        _basicAuthProcessorMock = GetMockBasicAuthProcessor();
        _groupProcessorMock = GetMockGroupProcessor();
        _roleProcessorMock = GetMockRoleProcessor();

        _registrationService = new RegistrationService(
            _serviceFactory.GetRequiredService<ILogger<RegistrationService>>(),
            _accountProcessorMock.Object,
            _userProcessorMock.Object,
            _basicAuthProcessorMock.Object,
            _groupProcessorMock.Object,
            _roleProcessorMock.Object,
            _unitOfWorkProviderMock.Object);
    }

    private Mock<IAccountProcessor> GetMockAccountProcessor()
    {
        var mock = new Mock<IAccountProcessor>();

        mock
            .Setup(m => m.CreateAccountAsync(
                It.IsAny<CreateAccountRequest>()))
            .ReturnsAsync(() =>
                new CreateAccountResult(_createAccountResultCode, string.Empty, _fixture.Create<int>()));

        return mock;
    }

    private Mock<IUserProcessor> GetMockUserProcessor()
    {
        var mock = new Mock<IUserProcessor>();

        mock
            .Setup(m => m.CreateUserAsync(
                It.IsAny<CreateUserRequest>()))
            .ReturnsAsync(() =>
                new CreateUserResult(_createUserResultCode, string.Empty, _fixture.Create<int>()));

        return mock;
    }

    private Mock<IBasicAuthProcessor> GetMockBasicAuthProcessor()
    {
        var mock = new Mock<IBasicAuthProcessor>();

        mock
            .Setup(m => m.UpsertBasicAuthAsync(
                It.IsAny<UpsertBasicAuthRequest>()))
            .ReturnsAsync(() =>
                new UpsertBasicAuthResult(_upsertBasicAuthResultCode, string.Empty,
                    _fixture.Create<int>(), _fixture.Create<UpsertType>()));

        return mock;
    }

    private Mock<IGroupProcessor> GetMockGroupProcessor()
    {
        var mock = new Mock<IGroupProcessor>();

        mock
            .Setup(m => m.CreateGroupAsync(
                It.IsAny<CreateGroupRequest>()))
            .ReturnsAsync(() =>
                new CreateGroupResult(_createGroupResultCode, string.Empty, _fixture.Create<int>()));

        mock
            .Setup(m => m.AddUsersToGroupAsync(
                It.IsAny<AddUsersToGroupRequest>()))
            .ReturnsAsync(() => _addUsersToGroupResult);

        mock
            .Setup(m => m.AssignRolesToGroupAsync(
                It.IsAny<AssignRolesToGroupRequest>()))
            .ReturnsAsync(() => _assignRolesToGroupResult);

        return mock;
    }

    private Mock<IRoleProcessor> GetMockRoleProcessor()
    {
        var mock = new Mock<IRoleProcessor>();

        mock
            .Setup(m => m.CreateRoleAsync(
                It.IsAny<CreateRoleRequest>()))
            .ReturnsAsync(() =>
                new CreateRoleResult(_createRoleResultCode, string.Empty, _fixture.Create<int>()));

        return mock;
    }

    [Fact]
    public async Task RegisterUserAsync_Succeeds_WhenAllServicesSucceed()
    {
        var request = _fixture.Create<RegisterUserRequest>();

        var result = await _registrationService.RegisterUserAsync(request);

        Assert.Equal(RegisterUserResultCode.Success, result.ResultCode);
    }

    [Fact]
    public async Task RegisterUserAsync_Fails_WhenUserProcessorFails()
    {
        _createUserResultCode = CreateUserResultCode.UserExistsError;
        var request = _fixture.Create<RegisterUserRequest>();

        var result = await _registrationService.RegisterUserAsync(request);

        Assert.Equal(RegisterUserResultCode.UserCreationError, result.ResultCode);
        _basicAuthProcessorMock.Verify(m => m.UpsertBasicAuthAsync(It.IsAny<UpsertBasicAuthRequest>()), Times.Never);
        _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_Fails_WhenBasicAuthProcessorFails()
    {
        _upsertBasicAuthResultCode = UpsertBasicAuthResultCode.Failure;
        var request = _fixture.Create<RegisterUserRequest>();

        var result = await _registrationService.RegisterUserAsync(request);

        Assert.Equal(RegisterUserResultCode.BasicAuthCreationError, result.ResultCode);
        _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _registrationService.RegisterUserAsync(null!));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task RegisterAccountAsync_Succeeds_WhenAllServicesSucceed()
    {
        var request = _fixture.Create<RegisterAccountRequest>();

        var result = await _registrationService.RegisterAccountAsync(request);

        Assert.Equal(CreateAccountResultCode.Success, result.ResultCode);
    }

    [Theory]
    [InlineData(CreateAccountResultCode.AccountExistsError)]
    [InlineData(CreateAccountResultCode.UserOwnerNotFoundError)]
    public async Task RegisterAccountAsync_Fails_WhenAccountProcessorFails(CreateAccountResultCode createAccountResultCode)
    {
        _createAccountResultCode = createAccountResultCode;
        var request = _fixture.Create<RegisterAccountRequest>();

        var result = await _registrationService.RegisterAccountAsync(request);

        Assert.Equal(createAccountResultCode, result.ResultCode);
    }

    [Fact]
    public async Task RegisterAccountAsync_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _registrationService.RegisterAccountAsync(null!));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task RegisterGroupAsync_Succeeds_WhenAllServicesSucceed()
    {
        var request = _fixture.Create<RegisterGroupRequest>();
        var result = await _registrationService.RegisterGroupAsync(request);
        Assert.Equal(CreateGroupResultCode.Success, result.ResultCode);
    }

    [Theory]
    [InlineData(CreateGroupResultCode.GroupExistsError)]
    [InlineData(CreateGroupResultCode.AccountNotFoundError)]
    public async Task RegisterGroupAsync_Fails_WhenGroupProcessorFails(CreateGroupResultCode createGroupResultCode)
    {
        _createGroupResultCode = createGroupResultCode;
        var request = _fixture.Create<RegisterGroupRequest>();

        var result = await _registrationService.RegisterGroupAsync(request);

        Assert.Equal(createGroupResultCode, result.ResultCode);
    }

    [Fact]
    public async Task RegisterGroupAsync_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _registrationService.RegisterGroupAsync(null!));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task RegisterRoleAsync_Succeeds_WhenAllServicesSucceed()
    {
        var request = _fixture.Create<RegisterRoleRequest>();
        var result = await _registrationService.RegisterRoleAsync(request);
        Assert.Equal(CreateRoleResultCode.Success, result.ResultCode);
    }

    [Theory]
    [InlineData(CreateRoleResultCode.RoleExistsError)]
    [InlineData(CreateRoleResultCode.AccountNotFoundError)]
    public async Task RegisterRoleAsync_Fails_WhenRoleProcessorFails(CreateRoleResultCode createRoleResultCode)
    {
        _createRoleResultCode = createRoleResultCode;
        var request = _fixture.Create<RegisterRoleRequest>();
        var result = await _registrationService.RegisterRoleAsync(request);
        Assert.Equal(createRoleResultCode, result.ResultCode);
    }

    [Fact]
    public async Task RegisterRoleAsync_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _registrationService.RegisterRoleAsync(null!));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task RegisterUsersWithGroupAsync_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _registrationService.RegisterUsersWithGroupAsync(null!));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task RegisterUsersWithGroupAsync_Succeeds_WhenAllServicesSucceed()
    {
        var request = _fixture.Create<RegisterUsersWithGroupRequest>();
        _addUsersToGroupResult = new(AddUsersToGroupResultCode.Success, string.Empty, request.GroupId);

        var result = await _registrationService.RegisterUsersWithGroupAsync(request);

        Assert.Equal(AddUsersToGroupResultCode.Success, result.ResultCode);
    }

    [Fact]
    public async Task RegisterUsersWithGroupAsync_Fails_WhenGroupProcessorFails()
    {
        var request = _fixture.Create<RegisterUsersWithGroupRequest>();
        _addUsersToGroupResult = new(AddUsersToGroupResultCode.GroupNotFoundError, "Error", _fixture.Create<int>(), _fixture.CreateMany<int>(5).ToList());

        var result = await _registrationService.RegisterUsersWithGroupAsync(request);

        Assert.Equal(AddUsersToGroupResultCode.GroupNotFoundError, result.ResultCode);
        Assert.Equal(_addUsersToGroupResult.Message, result.Message);
        Assert.Equal(_addUsersToGroupResult.AddedUserCount, result.RegisteredUserCount);
        Assert.Equal(_addUsersToGroupResult.InvalidUserIds.Count, result.InvalidUserIds.Count);
    }

    [Fact]
    public async Task RegisterRolesWithGroup_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _registrationService.RegisterRolesWithGroupAsync(null!));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task RegisterRolesWithGroup_Succeeds_WhenAllServicesSucceed()
    {
        var request = _fixture.Create<RegisterRolesWithGroupRequest>();
        _assignRolesToGroupResult = new(AssignRolesToGroupResultCode.Success, string.Empty, request.GroupId);

        var result = await _registrationService.RegisterRolesWithGroupAsync(request);

        Assert.Equal(AssignRolesToGroupResultCode.Success, result.ResultCode);
    }

    [Fact]
    public async Task RegisterRolesWithGroup_Fails_WhenGroupProcessorFails()
    {
        var request = _fixture.Create<RegisterRolesWithGroupRequest>();
        _assignRolesToGroupResult = new(AssignRolesToGroupResultCode.GroupNotFoundError, "Error", _fixture.Create<int>(), _fixture.CreateMany<int>(5).ToList());

        var result = await _registrationService.RegisterRolesWithGroupAsync(request);

        Assert.Equal(AssignRolesToGroupResultCode.GroupNotFoundError, result.ResultCode);
        Assert.Equal(_assignRolesToGroupResult.Message, result.Message);
        Assert.Equal(_assignRolesToGroupResult.AddedRoleCount, result.RegisteredRoleCount);
        Assert.Equal(_assignRolesToGroupResult.InvalidRoleIds.Count, result.InvalidRoleIds.Count);
    }
}
