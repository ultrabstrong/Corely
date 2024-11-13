using Corely.IAM.Groups.Entities;
using Corely.IAM.Groups.Models;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.GroupProfiles
{
    public class GroupProfileTests
        : BidirectionalProfileDelegateTestsBase
    {
        private class Delegate : BidirectionalProfileTestsBase<Group, GroupEntity>;

        protected override BidirectionalProfileTestsBase GetDelegate() => new Delegate();
    }
}
