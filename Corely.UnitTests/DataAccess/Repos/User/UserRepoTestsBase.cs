using AutoFixture;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;

namespace Corely.UnitTests.DataAccess.Repos.User
{
    public abstract class UserRepoTestsBase
    {
        protected readonly Fixture fixture = new();
        protected abstract IRepoExtendedGet<UserEntity> UserRepo { get; }

        public UserRepoTestsBase()
        {
            fixture.Customize<UserEntity>(c => c
                .Without(x => x.Details)
                .Without(x => x.BasicAuth));

            fixture.Customize<UserDetailsEntity>(c => c
                .Without(x => x.User));
        }

        [Fact]
        public async Task Create_ThenGet_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await UserRepo.CreateAsync(user);
            var result = await UserRepo.GetAsync(user.Id);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserName_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await UserRepo.CreateAsync(user);
            var result = await UserRepo.GetAsync(u => u.Username == user.Username);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetByEmail_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await UserRepo.CreateAsync(user);
            var result = await UserRepo.GetAsync(u => u.Email == user.Email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserNameOrEmail_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await UserRepo.CreateAsync(user);
            var result = await UserRepo.GetAsync(u
                => u.Username == user.Username || u.Email == user.Email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetWithDetailsById_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await UserRepo.CreateAsync(user);
            var result = await UserRepo.GetAsync(user.Id);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetWithDetailsByEmail_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await UserRepo.CreateAsync(user);
            var result = await UserRepo.GetAsync(u => u.Email == user.Email, u => u.Details!);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetWithDetailsByUserName_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await UserRepo.CreateAsync(user);
            var result = await UserRepo.GetAsync(u => u.Username == user.Username, u => u.Details!);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenUpdate_ShouldUpdateUser()
        {
            var user = fixture.Create<UserEntity>();

            await UserRepo.CreateAsync(user);
            user.Username = fixture.Create<string>();
            await UserRepo.UpdateAsync(user);
            var result = await UserRepo.GetAsync(user.Id);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenDelete_ShouldDeleteUser()
        {
            var user = fixture.Create<UserEntity>();

            await UserRepo.CreateAsync(user);
            await UserRepo.DeleteAsync(user);
            var result = await UserRepo.GetAsync(user.Id);

            Assert.Null(result);
        }
    }
}
