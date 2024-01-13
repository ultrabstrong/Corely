using Corely.DataAccess.Factories.AccountManagement;

namespace Corely.UnitTests.DataAccess.Factories.AccountManagement
{
    public class MockAccountManagementRepoFactoryTests : AccountManagementRepoFactoryTestsBase
    {
        private readonly MockAccountManagementRepoFactory _mockAccountManagementRepoFactory = new();
        protected override IAccountManagementRepoFactory AccountManagementRepoFactory
            => _mockAccountManagementRepoFactory;
    }
}
