using Corely.IAM.Users.Entities;
using Corely.IAM.Users.Models;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.UserProfiles
{
    public class UserDetailsProfileTests
        : BidirectionalProfileDelegateTestsBase
    {
        private class Delegate : BidirectionalProfileTestsBase<UserDetails, UserDetailsEntity>;

        protected override BidirectionalProfileTestsBase GetDelegate() => new Delegate();
    }
}
