using AutoFixture;
using Corely.DataAccess.Factories;

namespace Corely.UnitTests.DataAccess.Factories
{
    public abstract class AccountManagementRepoFactoryTestsBase
    {
        protected readonly Fixture fixture = new();
        protected abstract IAccountManagementRepoFactory AccountManagementRepoFactory { get; }

        [Fact]
        public void CreateAccountRepo_ShouldReturnMockAccountRepo()
        {
            var accountRepo = AccountManagementRepoFactory.CreateAccountRepo();
            Assert.NotNull(accountRepo);
        }

        [Fact]
        public void CreateReadonlyAccountRepo_ShouldReturnMockReadonlyAccountRepo()
        {
            var readonlyAccountRepo = AccountManagementRepoFactory.CreateReadonlyAccountRepo();
            Assert.NotNull(readonlyAccountRepo);
        }

        [Fact]
        public void CreateBasicAuthRepo_ShouldReturnMockBasicAuthRepo()
        {
            var basicAuthRepo = AccountManagementRepoFactory.CreateBasicAuthRepo();
            Assert.NotNull(basicAuthRepo);
        }

        [Fact]
        public void CreateUserRepo_ShouldReturnMockUserRepo()
        {
            var userRepo = AccountManagementRepoFactory.CreateUserRepo();
            Assert.NotNull(userRepo);
        }

        [Fact]
        public void CreateUnitOfWorkProvider_ShouldReturnUowProvider()
        {
            var uowProvider = AccountManagementRepoFactory.CreateUnitOfWorkProvider();
            Assert.NotNull(uowProvider);
        }
    }
}
