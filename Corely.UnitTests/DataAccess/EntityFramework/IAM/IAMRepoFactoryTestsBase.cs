using AutoFixture;
using Corely.DataAccess.EntityFramework.IAM;

namespace Corely.UnitTests.DataAccess.EntityFramework.IAM
{
    public abstract class IAMRepoFactoryTestsBase
    {
        protected readonly Fixture fixture = new();
        protected abstract IIAMRepoFactory AccountManagementRepoFactory { get; }

        [Fact]
        public void CreateAccountRepo_ReturnsMockAccountRepo()
        {
            var accountRepo = AccountManagementRepoFactory.CreateAccountRepo();
            Assert.NotNull(accountRepo);
        }

        [Fact]
        public void CreateReadonlyAccountRepo_ReturnsMockReadonlyAccountRepo()
        {
            var readonlyAccountRepo = AccountManagementRepoFactory.CreateReadonlyAccountRepo();
            Assert.NotNull(readonlyAccountRepo);
        }

        [Fact]
        public void CreateBasicAuthRepo_ReturnsMockBasicAuthRepo()
        {
            var basicAuthRepo = AccountManagementRepoFactory.CreateBasicAuthRepo();
            Assert.NotNull(basicAuthRepo);
        }

        [Fact]
        public void CreateUserRepo_ReturnsMockUserRepo()
        {
            var userRepo = AccountManagementRepoFactory.CreateUserRepo();
            Assert.NotNull(userRepo);
        }

        [Fact]
        public void CreateUnitOfWorkProvider_ReturnsUowProvider()
        {
            var uowProvider = AccountManagementRepoFactory.CreateUnitOfWorkProvider();
            Assert.NotNull(uowProvider);
        }
    }
}
