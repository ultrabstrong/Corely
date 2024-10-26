using Corely.IAM.DataAccess.EntityFramework;
using Corely.IAM.DataAccess.Mock;
using Corely.UnitTests.IAM.DataAccess.EntityFramework;

namespace Corely.UnitTests.IAM.DataAccess.Mock
{
    public class MockIAMRepoFactoryTests : IAMRepoFactoryTestsBase
    {
        private readonly MockIAMRepoFactory _mockIAMRepoFactory = new();
        protected override IIAMRepoFactory IAMRepoFactory
            => _mockIAMRepoFactory;
    }
}
