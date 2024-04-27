using AutoFixture;
using Corely.Security.Hashing;
using Corely.Security.Hashing.Models;
using Corely.Security.Hashing.Providers;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.SecurityProfiles
{
    public class HashedValueProfileTests
        : BidirectionalProfileTestsBase<HashedValue, string>
    {
        protected override string GetDestination()
            => $"{HashConstants.SALTED_SHA256_CODE}:{new Fixture().Create<string>()}";

        protected override object[] GetSourceParams()
            => [Mock.Of<IHashProvider>()];
    }
}
