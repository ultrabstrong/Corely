using AutoFixture;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Users;

namespace Corely.UnitTests.DataAccess.Repos.User
{
    public class MockUserRepoTests
    {
        private readonly MockUserRepo _mockUserRepo = new();
        private readonly Fixture _fixture = new();

        public MockUserRepoTests()
        {
            _fixture.Customize<UserEntity>(c => c
                .Without(x => x.Details)
                .Without(x => x.BasicAuth));
        }

        [Fact]
        public async Task Create_ThenGet_ShouldAddUser()
        {
            var user = _fixture.Create<UserEntity>();

            await _mockUserRepo.Create(user);
            var result = await _mockUserRepo.Get(user.Id);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserName_ShouldAddUser()
        {
            var user = _fixture.Create<UserEntity>();

            await _mockUserRepo.Create(user);
            var result = await _mockUserRepo.GetByUserName(user.Username);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetByEmail_ShouldAddUser()
        {
            var user = _fixture.Create<UserEntity>();

            await _mockUserRepo.Create(user);
            var result = await _mockUserRepo.GetByEmail(user.Email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserNameOrEmail_ShouldAddUser()
        {
            var user = _fixture.Create<UserEntity>();

            await _mockUserRepo.Create(user);
            var result = await _mockUserRepo.GetByUserNameOrEmail(user.Username, user.Email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetWithDetailsById_ShouldAddUser()
        {
            var user = _fixture.Create<UserEntity>();

            await _mockUserRepo.Create(user);
            var result = await _mockUserRepo.GetWithDetailsById(user.Id);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetWithDetailsByEmail_ShouldAddUser()
        {
            var user = _fixture.Create<UserEntity>();

            await _mockUserRepo.Create(user);
            var result = await _mockUserRepo.GetWithDetailsByEmail(user.Email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenGetWithDetailsByUserName_ShouldAddUser()
        {
            var user = _fixture.Create<UserEntity>();

            await _mockUserRepo.Create(user);
            var result = await _mockUserRepo.GetWithDetailsByUserName(user.Username);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenUpdate_ShouldUpdateUser()
        {
            var user = _fixture.Create<UserEntity>();

            await _mockUserRepo.Create(user);
            user.Username = _fixture.Create<string>();
            await _mockUserRepo.Update(user);
            var result = await _mockUserRepo.Get(user.Id);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Create_ThenDelete_ShouldDeleteUser()
        {
            var user = _fixture.Create<UserEntity>();

            await _mockUserRepo.Create(user);
            await _mockUserRepo.Delete(user);
            var result = await _mockUserRepo.Get(user.Id);

            Assert.Null(result);
        }
    }
}
