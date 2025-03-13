using Corely.IAM.Accounts.Models;
using Corely.UnitTests.Mappers.AutoMapper;

namespace Corely.UnitTests.Accounts.Mappers;

public class CreateAccountRequestProfileTests
    : ProfileDelegateTestsBase
{
    private class Delegate : ProfileTestsBase<CreateAccountRequest, Account>;

    protected override ProfileTestsBase GetDelegate() => new Delegate();
}
