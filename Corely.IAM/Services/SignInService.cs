using Corely.Common.Extensions;
using Corely.IAM.Auth.Services;
using Corely.IAM.Models;
using Corely.IAM.Users.Services;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Services
{
    internal class SignInService : ISignInService
    {
        private readonly ILogger<SignInService> _logger;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public SignInService(
            ILogger<SignInService> logger,
            IUserService userService,
            IAuthService authService)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _userService = userService.ThrowIfNull(nameof(userService));
            _authService = authService.ThrowIfNull(nameof(_authService));
        }

        public async Task<SignInResult> SignInAsync(SignInRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            _logger.LogDebug("Signing in user {Username}", request.Username);

            var user = await _userService.GetUserAsync(request.Username);
            if (user == null)
            {
                _logger.LogDebug("User {Username} not found", request.Username);
                return new SignInResult(false, "User not found", string.Empty);
            }

            // Todo : Add check for too many failed logins
            // May need to be controlled through some sort of new ISettingsProvider
            // Implement this after the other Todos are complete

            var isValidPassword = await _authService.VerifyBasicAuthAsync(new(user.Id, request.Password));
            if (!isValidPassword)
            {
                user.FailedLoginsSinceLastSuccess++;
                user.TotalFailedLogins++;
                user.LastFailedLoginUtc = DateTime.UtcNow;

                await _userService.UpdateUserAsync(user);

                _logger.LogDebug("User {Username} failed to sign in (invalid password)", request.Username);

                return new SignInResult(false, "Invalid password", string.Empty);
            }
            user.TotalSuccessfulLogins++;
            user.FailedLoginsSinceLastSuccess = 0;
            user.LastSuccessfulLoginUtc = DateTime.UtcNow;
            await _userService.UpdateUserAsync(user);

            var authToken = await _userService.GetUserAuthTokenAsync(user.Id);

            _logger.LogDebug("User {Username} signed in", request.Username);

            return new SignInResult(true, string.Empty, authToken);
        }
    }
}
