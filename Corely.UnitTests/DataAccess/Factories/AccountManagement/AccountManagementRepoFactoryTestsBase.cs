using AutoFixture;
using Corely.DataAccess.Factories.AccountManagement;

namespace Corely.UnitTests.DataAccess.Factories.AccountManagement
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
    }
}
