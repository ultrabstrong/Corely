using AutoFixture;
using Corely.Domain.Entities.Users;

namespace Corely.UnitTests.DataAccess.Repos.User
{
    public abstract class UserRepoTestsBase : RepoExtendedGetTestsBase<UserEntity>
    {
        public UserRepoTestsBase()
        {
            fixture.Customize<UserEntity>(c => c
                .Without(e => e.Details)
                .Without(e => e.BasicAuth)
                .Without(e => e.Accounts));

            fixture.Customize<UserDetailsEntity>(c => c
                .Without(e => e.User));
        }

        [Fact]
        public async Task Create_ThenGetByUserName_ShouldReturnAddedUser()
        {
            var user = fixture.Create<UserEntity>();

            await Repo.CreateAsync(user);
            var result = await Repo.GetAsync(u => u.Username == user.Username);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetByEmail_ShouldReturnAddedUser()
        {
            var user = fixture.Create<UserEntity>();

            await Repo.CreateAsync(user);
            var result = await Repo.GetAsync(u => u.Email == user.Email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserNameOrEmail_ShouldReturnAddedUser()
        {
            var user = fixture.Create<UserEntity>();

            await Repo.CreateAsync(user);
            var result = await Repo.GetAsync(u
                => u.Username == user.Username || u.Email == user.Email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetWithDetailsById_ShouldReturnAddedUser()
        {
            var user = fixture.Create<UserEntity>();

            await Repo.CreateAsync(user);
            var result = await Repo.GetAsync(
                u => u.Id == user.Id,
                u => u.Details!);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetWithDetailsByEmail_ShouldReturnAddedUser()
        {
            var user = fixture.Create<UserEntity>();

            await Repo.CreateAsync(user);
            var result = await Repo.GetAsync(
                u => u.Email == user.Email,
                u => u.Details!);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetWithDetailsByUserName_ShouldReturnAddedUser()
        {
            var user = fixture.Create<UserEntity>();

            await Repo.CreateAsync(user);
            var result = await Repo.GetAsync(u => u.Username == user.Username, u => u.Details!);

            Assert.Equal(user, result);
        }
    }
}
