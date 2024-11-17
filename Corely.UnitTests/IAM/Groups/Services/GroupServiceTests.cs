using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Exceptions;
using Corely.IAM.Groups.Entities;
using Corely.IAM.Groups.Exceptions;
using Corely.IAM.Groups.Models;
using Corely.IAM.Groups.Services;
using Corely.IAM.Mappers;
using Corely.IAM.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Groups.Services
{
    public class GroupServiceTests
    {
        private const string VALID_GROUP_NAME = "groupname";

        private readonly Fixture _fixture = new();
        private readonly ServiceFactory _serviceFactory = new();
        private readonly GroupService _groupService;

        public GroupServiceTests()
        {
            _groupService = new GroupService(
                _serviceFactory.GetRequiredService<IRepoExtendedGet<GroupEntity>>(),
                _serviceFactory.GetRequiredService<IReadonlyRepo<AccountEntity>>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<GroupService>>());
        }

        private async Task<int> CreateAccountAsync()
        {
            var accountId = _fixture.Create<int>();
            var account = new AccountEntity { Id = accountId };
            var accountRepo = _serviceFactory.GetRequiredService<IRepo<AccountEntity>>();
            return await accountRepo.CreateAsync(account);
        }

        [Fact]
        public async Task CreateGroupAsync_AccountDoesNotExistException_WhenAccountDoesNotExist()
        {
            var createGroupRequest = new CreateGroupRequest(VALID_GROUP_NAME, _fixture.Create<int>());

            var ex = await Record.ExceptionAsync(() => _groupService.CreateGroupAsync(createGroupRequest));

            Assert.NotNull(ex);
            Assert.IsType<AccountDoesNotExistException>(ex);
        }

        [Fact]
        public async Task CreateGroupAsync_Throws_WhenGroupExists()
        {
            var createGroupRequest = new CreateGroupRequest(VALID_GROUP_NAME, await CreateAccountAsync());
            await _groupService.CreateGroupAsync(createGroupRequest);

            var ex = await Record.ExceptionAsync(() => _groupService.CreateGroupAsync(createGroupRequest));

            Assert.NotNull(ex);
            Assert.IsType<GroupExistsException>(ex);
        }

        [Fact]
        public async Task CreateGroupAsync_ReturnsCreateGroupResult()
        {
            var accountId = await CreateAccountAsync();
            var createGroupRequest = new CreateGroupRequest(VALID_GROUP_NAME, accountId);

            var createGroupResult = await _groupService.CreateGroupAsync(createGroupRequest);

            Assert.True(createGroupResult.IsSuccess);

            // Verify group is linked to account id
            var groupRepo = _serviceFactory.GetRequiredService<IRepoExtendedGet<GroupEntity>>();
            var groupEntity = await groupRepo.GetAsync(
                g => g.Id == createGroupResult.CreatedId,
                include: q => q.Include(g => g.Account));
            Assert.NotNull(groupEntity);
            //Assert.NotNull(groupEntity.Account);
            Assert.Equal(accountId, groupEntity.AccountId);
        }

        [Fact]
        public async Task CreateGroupAsync_Throws_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _groupService.CreateGroupAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task CreateGroupAsync_Throws_WithNullGroupName()
        {
            var createGroupRequest = new CreateGroupRequest(null!, await CreateAccountAsync());

            var ex = await Record.ExceptionAsync(() => _groupService.CreateGroupAsync(createGroupRequest));

            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
        }
    }
}
