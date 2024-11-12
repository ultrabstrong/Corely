using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Models;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.AccountProfiles
{
    public class AccountProfileTests
        : BidirectionalProfileDelegateTestsBase
    {
        private class Delegate : BidirectionalProfileTestsBase<Account, AccountEntity>;

        protected override BidirectionalProfileTestsBase GetDelegate() => new Delegate();
    }
}
