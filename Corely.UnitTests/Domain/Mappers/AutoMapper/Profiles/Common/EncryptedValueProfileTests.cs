using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Encryption;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.Profiles.Common
{
    public class EncryptedValueProfileTests
        : BidirectionalAutoMapperTestsBase<EncryptedValue, string>
    {
        protected override object[] GetSourceParams() => [Mock.Of<IEncryptionProvider>()];
    }
}
