using Corely.DataAccess.EntityFramework.IAM;
using Corely.DataAccess.Mock;
using Corely.UnitTests.DataAccess.EntityFramework.IAM;

namespace Corely.UnitTests.DataAccess.Mock
{
    public class MockAccountManagementRepoFactoryTests : IAMRepoFactoryTestsBase
    {
        private readonly MockAccountManagementRepoFactory _mockAccountManagementRepoFactory = new();
        protected override IIAMRepoFactory AccountManagementRepoFactory
            => _mockAccountManagementRepoFactory;
    }
}
