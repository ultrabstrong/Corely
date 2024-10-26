using AutoFixture;
using Corely.IAM.DataAccess.EntityFramework;

namespace Corely.UnitTests.IAM.DataAccess.EntityFramework
{
    public abstract class IAMRepoFactoryTestsBase
    {
        protected readonly Fixture fixture = new();
        protected abstract IIAMRepoFactory IAMRepoFactory { get; }

        [Fact]
        public void CreateAccountRepo_ReturnsMockAccountRepo()
        {
            var accountRepo = IAMRepoFactory.CreateAccountRepo();
            Assert.NotNull(accountRepo);
        }

        [Fact]
        public void CreateReadonlyAccountRepo_ReturnsMockReadonlyAccountRepo()
        {
            var readonlyAccountRepo = IAMRepoFactory.CreateReadonlyAccountRepo();
            Assert.NotNull(readonlyAccountRepo);
        }

        [Fact]
        public void CreateBasicAuthRepo_ReturnsMockBasicAuthRepo()
        {
            var basicAuthRepo = IAMRepoFactory.CreateBasicAuthRepo();
            Assert.NotNull(basicAuthRepo);
        }

        [Fact]
        public void CreateUserRepo_ReturnsMockUserRepo()
        {
            var userRepo = IAMRepoFactory.CreateUserRepo();
            Assert.NotNull(userRepo);
        }

        [Fact]
        public void CreateReadonlyUserRepo_ReturnsMockReadonlyUserRepo()
        {
            var readonlyUserRepo = IAMRepoFactory.CreateReadonlyUserRepo();
            Assert.NotNull(readonlyUserRepo);
        }

        [Fact]
        public void CreateUnitOfWorkProvider_ReturnsUowProvider()
        {
            var uowProvider = IAMRepoFactory.CreateUnitOfWorkProvider();
            Assert.NotNull(uowProvider);
        }
    }
}
