using AutoFixture;
using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Hashing;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.Profiles.Common
{
    public class HashedValueProfileTests
        : BidirectionalAutoMapperTestsBase<HashedValue, string>
    {
        protected override string GetDestination()
            => $"{HashProviderConstants.SALTED_SHA256_CODE}:{new Fixture().Create<string>()}";

        protected override object[] GetSourceParams()
            => [Mock.Of<IHashProvider>()];
    }
}
