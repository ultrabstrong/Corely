using AutoFixture;
using Corely.DataAccess.Repos.Auth;
using Corely.Domain.Entities.Auth;

namespace Corely.UnitTests.DataAccess.Repos.Auth
{
    public class MockBasicAuthRepoTests
    {
        private readonly MockBasicAuthRepo _mockBasicAuthRepo = new();
        private readonly Fixture _fixture = new();

        public MockBasicAuthRepoTests()
        {
            _fixture.Customize<BasicAuthEntity>(c => c
                .Without(x => x.User));
        }

        [Fact]
        public async Task Create_ThenGet_ShouldAddBasicAuth()
        {
            var basicAuth = _fixture.Create<BasicAuthEntity>();

            await _mockBasicAuthRepo.Create(basicAuth);
            var result = await _mockBasicAuthRepo.Get(basicAuth.Id);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserId_ShouldAddBasicAuth()
        {
            var basicAuth = _fixture.Create<BasicAuthEntity>();

            await _mockBasicAuthRepo.Create(basicAuth);
            var result = await _mockBasicAuthRepo.GetByUserId(basicAuth.UserId);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserName_ShouldAddBasicAuth()
        {
            var basicAuth = _fixture.Create<BasicAuthEntity>();

            await _mockBasicAuthRepo.Create(basicAuth);
            var result = await _mockBasicAuthRepo.GetByUserName(basicAuth.Username);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenUpdate_ShouldUpdateBasicAuth()
        {
            var basicAuth = _fixture.Create<BasicAuthEntity>();

            await _mockBasicAuthRepo.Create(basicAuth);
            basicAuth.Username = "newUsername";
            await _mockBasicAuthRepo.Update(basicAuth);
            var result = await _mockBasicAuthRepo.Get(basicAuth.Id);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenDelete_ShouldDeleteBasicAuth()
        {
            var basicAuth = _fixture.Create<BasicAuthEntity>();

            await _mockBasicAuthRepo.Create(basicAuth);
            await _mockBasicAuthRepo.Delete(basicAuth);
            var result = await _mockBasicAuthRepo.Get(basicAuth.Id);

            Assert.Null(result);
        }
    }
}
