using Corely.IAM.DataAccess.EntityFramework;
using Corely.IAM.DataAccess.Mock;
using Corely.UnitTests.DataAccess.EntityFramework.IAM;

namespace Corely.UnitTests.DataAccess.Mock
{
    public class MockIAMRepoFactoryTests : IAMRepoFactoryTestsBase
    {
        private readonly MockIAMRepoFactory _mockIAMRepoFactory = new();
        protected override IIAMRepoFactory IAMRepoFactory
            => _mockIAMRepoFactory;
    }
}
