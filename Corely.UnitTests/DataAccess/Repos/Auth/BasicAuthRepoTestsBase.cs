using AutoFixture;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;

namespace Corely.UnitTests.DataAccess.Repos.Auth
{
    public abstract class BasicAuthRepoTestsBase
    {
        protected readonly Fixture fixture = new();
        protected abstract IAuthRepo<BasicAuthEntity> MockBasicAuthRepo { get; }

        public BasicAuthRepoTestsBase()
        {
            fixture.Customize<BasicAuthEntity>(c => c
                .Without(x => x.User));
        }

        [Fact]
        public async Task Create_ThenGet_ShouldAddBasicAuth()
        {
            var basicAuth = fixture.Create<BasicAuthEntity>();

            await MockBasicAuthRepo.Create(basicAuth);
            var result = await MockBasicAuthRepo.Get(basicAuth.Id);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserId_ShouldAddBasicAuth()
        {
            var basicAuth = fixture.Create<BasicAuthEntity>();

            await MockBasicAuthRepo.Create(basicAuth);
            var result = await MockBasicAuthRepo.GetByUserId(basicAuth.UserId);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserName_ShouldAddBasicAuth()
        {
            var basicAuth = fixture.Create<BasicAuthEntity>();

            await MockBasicAuthRepo.Create(basicAuth);
            var result = await MockBasicAuthRepo.GetByUserName(basicAuth.Username);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenUpdate_ShouldUpdateBasicAuth()
        {
            var basicAuth = fixture.Create<BasicAuthEntity>();

            await MockBasicAuthRepo.Create(basicAuth);
            basicAuth.Username = "newUsername";
            await MockBasicAuthRepo.Update(basicAuth);
            var result = await MockBasicAuthRepo.Get(basicAuth.Id);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenDelete_ShouldDeleteBasicAuth()
        {
            var basicAuth = fixture.Create<BasicAuthEntity>();

            await MockBasicAuthRepo.Create(basicAuth);
            await MockBasicAuthRepo.Delete(basicAuth);
            var result = await MockBasicAuthRepo.Get(basicAuth.Id);

            Assert.Null(result);
        }
    }
}
