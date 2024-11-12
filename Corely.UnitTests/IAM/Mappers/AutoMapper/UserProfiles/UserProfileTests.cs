using Corely.IAM.Users.Entities;
using Corely.IAM.Users.Models;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.UserProfiles
{
    public class UserProfileTests
        : BidirectionalProfileDelegateTestsBase
    {
        private class Delegate : BidirectionalProfileTestsBase<User, UserEntity>;

        protected override BidirectionalProfileTestsBase GetDelegate() => new Delegate();
    }
}
