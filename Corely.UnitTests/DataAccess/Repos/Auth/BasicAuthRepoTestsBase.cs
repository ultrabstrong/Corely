using AutoFixture;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;

namespace Corely.UnitTests.DataAccess.Repos.Auth
{
    public abstract class BasicAuthRepoTestsBase
    {
        protected readonly Fixture fixture = new();
        protected abstract IRepoExtendedGet<BasicAuthEntity> BasicAuthRepo { get; }

        public BasicAuthRepoTestsBase()
        {
            fixture.Customize<BasicAuthEntity>(c => c
                .Without(x => x.User));
        }

        [Fact]
        public async Task Create_ThenGet_ShouldAddBasicAuth()
        {
            var basicAuth = fixture.Create<BasicAuthEntity>();

            await BasicAuthRepo.CreateAsync(basicAuth);
            var result = await BasicAuthRepo.GetAsync(basicAuth.Id);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserId_ShouldAddBasicAuth()
        {
            var basicAuth = fixture.Create<BasicAuthEntity>();

            await BasicAuthRepo.CreateAsync(basicAuth);
            var result = await BasicAuthRepo.GetAsync(a => a.UserId == basicAuth.UserId);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserName_ShouldAddBasicAuth()
        {
            var basicAuth = fixture.Create<BasicAuthEntity>();

            await BasicAuthRepo.CreateAsync(basicAuth);
            var result = await BasicAuthRepo.GetAsync(a => a.Username == basicAuth.Username);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenUpdate_ShouldUpdateBasicAuth()
        {
            var basicAuth = fixture.Create<BasicAuthEntity>();

            await BasicAuthRepo.CreateAsync(basicAuth);
            basicAuth.Username = "newUsername";
            await BasicAuthRepo.UpdateAsync(basicAuth);
            var result = await BasicAuthRepo.GetAsync(basicAuth.Id);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenDelete_ShouldDeleteBasicAuth()
        {
            var basicAuth = fixture.Create<BasicAuthEntity>();

            await BasicAuthRepo.CreateAsync(basicAuth);
            await BasicAuthRepo.DeleteAsync(basicAuth);
            var result = await BasicAuthRepo.GetAsync(basicAuth.Id);

            Assert.Null(result);
        }
    }
}
