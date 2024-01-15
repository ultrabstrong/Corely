using Corely.Domain.Models.Users;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.Profiles.Users
{
    public class CreateUserRequestTests
    {
        public class ToUserTests
            : ProfileTestsBase<CreateUserRequest, User>
        {
        }
    }
}
