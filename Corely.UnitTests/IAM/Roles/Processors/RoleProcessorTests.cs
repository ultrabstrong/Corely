using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Groups.Processors;
using Corely.IAM.Mappers;
using Corely.IAM.Roles.Entities;
using Corely.IAM.Roles.Models;
using Corely.IAM.Roles.Processors;
using Corely.IAM.Validators;
using Corely.UnitTests.ClassData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Roles.Processors;
public class RoleProcessorTests
{
    private const string VALID_ROLE_NAME = "rolename";

    private readonly Fixture _fixture = new();
    private readonly ServiceFactory _serviceFactory = new();
    private readonly RoleProcessor _roleProcessor;

    public RoleProcessorTests()
    {
        _roleProcessor = new RoleProcessor(
            _serviceFactory.GetRequiredService<IRepo<RoleEntity>>(),
            _serviceFactory.GetRequiredService<IReadonlyRepo<AccountEntity>>(),
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

    [Fact]
    public async Task CreateRoleAsync_Fails_WhenAccountDoesNotExist()
    {
        var request = new CreateRoleRequest(VALID_ROLE_NAME, _fixture.Create<int>());

        var result = await _roleProcessor.CreateRoleAsync(request);

        Assert.Equal(CreateRoleResultCode.AccountNotFoundError, result.ResultCode);
    }

    [Fact]
    public async Task CreateRoleAsync_Fails_WhenRoleExists()
    {
        var request = new CreateRoleRequest(VALID_ROLE_NAME, await CreateAccountAsync());
        await _roleProcessor.CreateRoleAsync(request);

        var result = await _roleProcessor.CreateRoleAsync(request);

        Assert.Equal(CreateRoleResultCode.RoleExistsError, result.ResultCode);
    }

    [Fact]
    public async Task CreateRoleAsync_ReturnsCreateRoleResult()
    {
        var accountId = await CreateAccountAsync();
        var request = new CreateRoleRequest(VALID_ROLE_NAME, accountId);

        var result = await _roleProcessor.CreateRoleAsync(request);

        Assert.Equal(CreateRoleResultCode.Success, result.ResultCode);

        // Verify role is linked to account id
        var roleRepo = _serviceFactory.GetRequiredService<IRepo<RoleEntity>>();
        var roleEntity = await roleRepo.GetAsync(
            r => r.Id == result.CreatedId,
            include: q => q.Include(r => r.Account));
        Assert.NotNull(roleEntity);
        //Assert.NotNull(roleEntity.Account); // Account not available for memory mock repo
        Assert.Equal(accountId, roleEntity.AccountId);
    }

    [Fact]
    public async Task CreateRoleAsync_Throws_WithNullRequest()
    {
        var ex = Record.ExceptionAsync(() => _roleProcessor.CreateRoleAsync(null!));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(await ex);
    }

    [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
    public async Task CreateRoleAsync_Throws_WhenRoleNameInvalid(string roleName)
    {
        var request = new CreateRoleRequest(roleName, await CreateAccountAsync());

        var ex = Record.ExceptionAsync(() => _roleProcessor.CreateRoleAsync(request));

        Assert.NotNull(ex);
        Assert.IsType<ValidationException>(await ex);
    }
}
