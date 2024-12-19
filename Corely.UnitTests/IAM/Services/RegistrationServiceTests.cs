using AutoFixture;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Accounts.Processors;
using Corely.IAM.BasicAuths.Models;
using Corely.IAM.BasicAuths.Processors;
using Corely.IAM.Enums;
using Corely.IAM.Groups.Enums;
using Corely.IAM.Groups.Models;
using Corely.IAM.Groups.Processors;
using Corely.IAM.Models;
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
    private readonly RegistrationService _registrationService;

    private bool _createAccountSuccess = true;
    private bool _createUserSuccess = true;
    private bool _createBasicAuthSuccess = true;
    private bool _createGroupSuccess = true;

    private AddUsersToGroupResult _addUsersToGroupResult = new(AddUsersToGroupResultCode.Success, string.Empty, 0);

    public RegistrationServiceTests() : base()
    {
        _accountProcessorMock = GetMockAccountProcessor();
        _userProcessorMock = GetMockUserProcessor();
        _basicAuthProcessorMock = GetMockBasicAuthProcessor();
        _groupProcessorMock = GetMockGroupProcessor();

        _registrationService = new RegistrationService(
            _serviceFactory.GetRequiredService<ILogger<RegistrationService>>(),
            _accountProcessorMock.Object,
            _userProcessorMock.Object,
            _basicAuthProcessorMock.Object,
            _groupProcessorMock.Object,
            _unitOfWorkProviderMock.Object);
    }

    private Mock<IAccountProcessor> GetMockAccountProcessor()
    {
        var accountProcessorMock = new Mock<IAccountProcessor>();

        accountProcessorMock
            .Setup(m => m.CreateAccountAsync(
                It.IsAny<CreateAccountRequest>()))
            .ReturnsAsync(() =>
                new CreateResult(_createAccountSuccess, string.Empty, _fixture.Create<int>()));

        return accountProcessorMock;
    }

    private Mock<IUserProcessor> GetMockUserProcessor()
    {
        var userProcessorMock = new Mock<IUserProcessor>();

        userProcessorMock
            .Setup(m => m.CreateUserAsync(
                It.IsAny<CreateUserRequest>()))
            .ReturnsAsync(() =>
                new CreateResult(_createUserSuccess, string.Empty, _fixture.Create<int>()));

        return userProcessorMock;
    }

    private Mock<IBasicAuthProcessor> GetMockBasicAuthProcessor()
    {
        var basicAuthProcessorMock = new Mock<IBasicAuthProcessor>();

        basicAuthProcessorMock
            .Setup(m => m.UpsertBasicAuthAsync(
                It.IsAny<UpsertBasicAuthRequest>()))
            .ReturnsAsync(() =>
                new UpsertBasicAuthResult(_createBasicAuthSuccess, string.Empty,
                    _fixture.Create<int>(), _fixture.Create<UpsertType>()));

        return basicAuthProcessorMock;
    }

    private Mock<IGroupProcessor> GetMockGroupProcessor()
    {
        var groupProcessorMock = new Mock<IGroupProcessor>();

        groupProcessorMock
            .Setup(m => m.CreateGroupAsync(
                It.IsAny<CreateGroupRequest>()))
            .ReturnsAsync(() =>
                new CreateResult(_createGroupSuccess, string.Empty, _fixture.Create<int>()));

        groupProcessorMock
            .Setup(m => m.AddUsersToGroupAsync(
                It.IsAny<AddUsersToGroupRequest>()))
            .ReturnsAsync(() => _addUsersToGroupResult);

        return groupProcessorMock;
    }

    [Fact]
    public async Task RegisterUserAsync_ReturnsSuccessResult_WhenAllServicesSucceed()
    {
        var request = _fixture.Create<RegisterUserRequest>();

        var result = await _registrationService.RegisterUserAsync(request);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RegisterUserAsync_ReturnsFailureResult_WhenUserProcessorFails()
    {
        _createUserSuccess = false;
        var request = _fixture.Create<RegisterUserRequest>();

        var result = await _registrationService.RegisterUserAsync(request);

        Assert.False(result.IsSuccess);
        _basicAuthProcessorMock.Verify(m => m.UpsertBasicAuthAsync(It.IsAny<UpsertBasicAuthRequest>()), Times.Never);
        _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_ReturnsFailureResult_WhenBasicAuthProcessorFails()
    {
        _createBasicAuthSuccess = false;
        var request = _fixture.Create<RegisterUserRequest>();

        var result = await _registrationService.RegisterUserAsync(request);

        Assert.False(result.IsSuccess);
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
    public async Task RegisterAccountAsync_ReturnsSuccessResult_WhenAllServicesSucceed()
    {
        var request = _fixture.Create<RegisterAccountRequest>();

        var result = await _registrationService.RegisterAccountAsync(request);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RegisterAccountAsync_ReturnsFailureResult_WhenAccountProcessorFails()
    {
        _createAccountSuccess = false;
        var request = _fixture.Create<RegisterAccountRequest>();

        var result = await _registrationService.RegisterAccountAsync(request);

        Assert.False(result.IsSuccess);
        _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterAccountAsync_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _registrationService.RegisterAccountAsync(null!));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task RegisterGroupAsync_ReturnsSuccessResult_WhenAllServicesSucceed()
    {
        var request = _fixture.Create<RegisterGroupRequest>();
        var result = await _registrationService.RegisterGroupAsync(request);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RegisterGroupAsync_ReturnsFailureResult_WhenGroupProcessorFails()
    {
        _createGroupSuccess = false;
        var request = _fixture.Create<RegisterGroupRequest>();

        var result = await _registrationService.RegisterGroupAsync(request);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task RegisterGroupAsync_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _registrationService.RegisterGroupAsync(null!));
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
    public async Task RegisterUsersWithGroupAsync_ReturnsSuccessResult_WhenAllServicesSucceed()
    {
        var request = _fixture.Create<RegisterUsersWithGroupRequest>();
        _addUsersToGroupResult = new(AddUsersToGroupResultCode.Success, string.Empty, request.GroupId);

        var result = await _registrationService.RegisterUsersWithGroupAsync(request);

        Assert.Equal(AddUsersToGroupResultCode.Success, result.ResultCode);
    }

    [Fact]
    public async Task RegisterUsersWithGroupAsync_ReturnsFailureResult_WhenGroupProcessorFails()
    {
        var request = _fixture.Create<RegisterUsersWithGroupRequest>();
        _addUsersToGroupResult = new(AddUsersToGroupResultCode.GroupNotFoundError, "Error", _fixture.Create<int>(), _fixture.CreateMany<int>(5).ToList());

        var result = await _registrationService.RegisterUsersWithGroupAsync(request);

        Assert.Equal(AddUsersToGroupResultCode.GroupNotFoundError, result.ResultCode);
        Assert.Equal(_addUsersToGroupResult.Message, result.Message);
        Assert.Equal(_addUsersToGroupResult.AddedUserCount, result.RegisteredUserCount);
        Assert.Equal(_addUsersToGroupResult.InvalidUserIds.Count, result.InvalidUserIds.Count);
    }
}
