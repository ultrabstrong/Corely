using Corely.Domain.Constants.Users;

namespace Corely.Domain.Constants.Auth
{
    public static class BasicAuthConstants
    {
        public const int USERNAME_MIN_LENGTH = UserConstants.USERNAME_MIN_LENGTH;
        public const int USERNAME_MAX_LENGTH = UserConstants.USERNAME_MAX_LENGTH;
        public const int PASSWORD_MAX_LENGTH = 250; // Hashed password with encoded salt / other info
    }
}
