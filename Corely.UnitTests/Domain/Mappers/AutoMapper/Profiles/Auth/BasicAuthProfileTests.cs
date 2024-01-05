using Corely.Common.Providers.Security.Hashing;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Models.Auth;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.Profiles.Auth
{
    public class BasicAuthProfileTests
        : BidirectionalAutoMapperTestsBase<BasicAuth, BasicAuthEntity>
    {
        protected override BasicAuthEntity ApplyDestinatonModifications(BasicAuthEntity destination)
        {
            destination.Password = $"{HashProviderConstants.SALTED_SHA256_CODE}:{destination.Password}";
            return destination;
        }
    }
}
