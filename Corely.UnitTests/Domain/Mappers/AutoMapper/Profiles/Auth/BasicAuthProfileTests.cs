using Corely.Domain.Entities.Auth;
using Corely.Domain.Models.Auth;
using Corely.Security.Hashing;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.Profiles.Auth
{
    public class BasicAuthProfileTests
        : BidirectionalProfileTestsBase<BasicAuth, BasicAuthEntity>
    {
        protected override BasicAuthEntity ApplyDestinatonModifications(BasicAuthEntity destination)
        {
            destination.Password = $"{HashConstants.SALTED_SHA256_CODE}:{destination.Password}";
            return destination;
        }
    }
}
