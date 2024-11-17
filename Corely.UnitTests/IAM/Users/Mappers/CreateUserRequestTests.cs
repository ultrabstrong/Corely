using Corely.IAM.Users.Models;
using Corely.UnitTests.IAM.Mappers.AutoMapper;

namespace Corely.UnitTests.IAM.Users.Mappers
{
    public class CreateUserRequestTests
        : ProfileDelegateTestsBase
    {
        private class Delegate : ProfileTestsBase<CreateUserRequest, User>;

        protected override ProfileTestsBase GetDelegate() => new Delegate();
    }
}
