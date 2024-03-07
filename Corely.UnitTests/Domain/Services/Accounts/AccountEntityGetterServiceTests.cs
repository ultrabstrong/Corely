using AutoFixture;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Repos;
using Corely.Domain.Services.Accounts;

namespace Corely.UnitTests.Domain.Services.Accounts
{
    public class AccountEntityGetterServiceTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IRepoExtendedGet<AccountEntity>> _accountRepoMock = new();
        private readonly AccountEntityGetterService _accountEntityGetterService;

        public AccountEntityGetterServiceTests()
        {
            _accountEntityGetterService = new AccountEntityGetterService(_accountRepoMock.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenAccountRepoIsNull()
        {
            var ex = Record.Exception(() => new AccountEntityGetterService(null));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task GetAsync_ShouldCallRepoGetAsync()
        {
            var id = _fixture.Create<int>();

            await _accountEntityGetterService.GetAsync(id);

            _accountRepoMock.Verify(r => r.GetAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
