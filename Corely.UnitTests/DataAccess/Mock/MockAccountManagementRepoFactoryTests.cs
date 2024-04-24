using Corely.DataAccess.Factories;
using Corely.DataAccess.Mock;
using Corely.UnitTests.DataAccess.Factories;

namespace Corely.UnitTests.DataAccess.Mock
{
    public class MockAccountManagementRepoFactoryTests : AccountManagementRepoFactoryTestsBase
    {
        private readonly MockAccountManagementRepoFactory _mockAccountManagementRepoFactory = new();
        protected override IAccountManagementRepoFactory AccountManagementRepoFactory
            => _mockAccountManagementRepoFactory;
    }
}
