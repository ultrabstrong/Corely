using Corely.IAM.BasicAuths.Entities;
using Corely.Security.Hashing;
using Corely.UnitTests.IAM.Mappers.AutoMapper;

namespace Corely.UnitTests.IAM.BasicAuths.Mappers
{
    public class BasicAuthProfileTests
        : BidirectionalProfileDelegateTestsBase
    {
        private class Delegate : BidirectionalProfileTestsBase<Corely.IAM.BasicAuths.Models.BasicAuth, BasicAuthEntity>
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
