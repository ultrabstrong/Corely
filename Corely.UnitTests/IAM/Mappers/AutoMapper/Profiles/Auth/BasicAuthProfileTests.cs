using Corely.IAM.Entities.Auth;
using Corely.IAM.Models.Auth;
using Corely.Security.Hashing;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.Profiles.Auth
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
