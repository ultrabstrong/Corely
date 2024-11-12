using Corely.IAM.Auth.Entities;
using Corely.IAM.Auth.Models;
using Corely.Security.Hashing;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.AuthProfiles
{
    public class BasicAuthProfileTests
        : BidirectionalProfileDelegateTestsBase
    {
        private class Delegate : BidirectionalProfileTestsBase<BasicAuth, BasicAuthEntity>
        {
            protected override BasicAuthEntity ApplyDestinatonModifications(BasicAuthEntity destination)
            {
                destination.Password = $"{HashConstants.SALTED_SHA256_CODE}:{destination.Password}";
                return destination;
            }
        }

        protected override BidirectionalProfileTestsBase GetDelegate() => new Delegate();
    }
}
