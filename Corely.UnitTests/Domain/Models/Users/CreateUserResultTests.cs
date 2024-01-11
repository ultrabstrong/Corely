using AutoFixture;
using Corely.Domain.Models.Users;

namespace Corely.UnitTests.Domain.Models.Users
{
    public class CreateUserResultTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void Properties_ShouldInitAndBeAccessible()
        {
            var message = _fixture.Create<string>();
            var createUserResult = new CreateUserResult(true, message);

            Assert.True(createUserResult.IsSuccess);
            Assert.Equal(message, createUserResult.Message);
        }
    }
}
