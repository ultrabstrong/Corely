using AutoFixture;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;

namespace Corely.UnitTests.DataAccess.Repos.User
{
    public abstract class UserRepoTestsBase
    {
        protected readonly Fixture fixture = new();
        protected abstract IUserRepo MockUserRepo { get; }

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

            await MockUserRepo.Create(user);
            var result = await MockUserRepo.Get(user.Id);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserName_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await MockUserRepo.Create(user);
            var result = await MockUserRepo.GetByUserName(user.Username);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetByEmail_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await MockUserRepo.Create(user);
            var result = await MockUserRepo.GetByEmail(user.Email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserNameOrEmail_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await MockUserRepo.Create(user);
            var result = await MockUserRepo.GetByUserNameOrEmail(user.Username, user.Email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetWithDetailsById_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await MockUserRepo.Create(user);
            var result = await MockUserRepo.GetWithDetailsById(user.Id);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetWithDetailsByEmail_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await MockUserRepo.Create(user);
            var result = await MockUserRepo.GetWithDetailsByEmail(user.Email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetWithDetailsByUserName_ShouldAddUser()
        {
            var user = fixture.Create<UserEntity>();

            await MockUserRepo.Create(user);
            var result = await MockUserRepo.GetWithDetailsByUserName(user.Username);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenUpdate_ShouldUpdateUser()
        {
            var user = fixture.Create<UserEntity>();

            await MockUserRepo.Create(user);
            user.Username = fixture.Create<string>();
            await MockUserRepo.Update(user);
            var result = await MockUserRepo.Get(user.Id);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenDelete_ShouldDeleteUser()
        {
            var user = fixture.Create<UserEntity>();

            await MockUserRepo.Create(user);
            await MockUserRepo.Delete(user);
            var result = await MockUserRepo.Get(user.Id);

            Assert.Null(result);
        }
    }
}
