using AutoFixture;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Repos;
using Corely.Domain.Services.Accounts;

namespace Corely.UnitTests.Domain.Services.Accounts
{
    public class AccountEntityReadonlyServiceTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IRepoExtendedGet<AccountEntity>> _accountRepoMock = new();
        private readonly AccountEntityReadonlyService _accountEntityReadonlyService;

        public AccountEntityReadonlyServiceTests()
        {
            _accountEntityReadonlyService = new AccountEntityReadonlyService(_accountRepoMock.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenAccountRepoIsNull()
        {
            var ex = Record.Exception(() => new AccountEntityReadonlyService(null));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task GetAsync_ShouldCallRepoGetAsync()
        {
            var id = _fixture.Create<int>();

            await _accountEntityReadonlyService.GetAsync(id);

            _accountRepoMock.Verify(r => r.GetAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
