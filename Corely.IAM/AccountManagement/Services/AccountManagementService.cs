using Corely.Common.Extensions;
using Corely.IAM.AccountManagement.Models;
using Corely.IAM.Accounts.Services;
using Corely.IAM.Auth.Services;
using Corely.IAM.Repos;
using Corely.IAM.Users.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.AccountManagement.Services
{
    internal class AccountManagementService : IAccountManagementService
    {
        private readonly ILogger<AccountManagementService> _logger;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IUnitOfWorkProvider _uowProvider;

        public AccountManagementService(
            ILogger<AccountManagementService> logger,
            IAccountService accountService,
            IUserService userService,
            IAuthService authService,
            [FromKeyedServices(nameof(IAccountManagementService))]
            IUnitOfWorkProvider uowProvider)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _accountService = accountService.ThrowIfNull(nameof(accountService));
            _userService = userService.ThrowIfNull(nameof(userService));
            _authService = authService.ThrowIfNull(nameof(authService));
            _uowProvider = uowProvider.ThrowIfNull(nameof(uowProvider));
        }

        public async Task<RegisterResult> RegisterAsync(RegisterRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            _logger.LogInformation("Registering account {Account}", request.AccountName);

            bool operationSucceeded = false;
            try
            {
                await _uowProvider.BeginAsync();
                var createAccountResult = await _accountService.CreateAccountAsync(new(request.AccountName));
                if (!createAccountResult.IsSuccess)
                {
                    _logger.LogInformation("Creating account failed for {Account}", request.AccountName);
                    return FailedRegistrationResult();
                }

                var createUserResult = await _userService.CreateUserAsync(new(createAccountResult.CreatedId, request.Username, request.Email));
                if (!createUserResult.IsSuccess)
                {
                    _logger.LogInformation("Creating user failed for account {Account}", request.AccountName);
                    return FailedRegistrationResult();
                }

                var createAuthResult = await _authService.UpsertBasicAuthAsync(new(createUserResult.CreatedId, request.Password));
                if (!createAuthResult.IsSuccess)
                {
                    _logger.LogInformation("Creating auth failed for user {Username}", request.Username);
                    return FailedRegistrationResult();
                }

                await _uowProvider.CommitAsync();
                operationSucceeded = true;
                _logger.LogInformation("Account {Account} registered with Id {Id}", request.AccountName, createAccountResult.CreatedId);
                return new RegisterResult(true, string.Empty, createAccountResult.CreatedId, createUserResult.CreatedId, createAuthResult.CreatedId);
            }
            finally
            {
                if (!operationSucceeded)
                {
                    await _uowProvider.RollbackAsync();
                }
            }
        }

        private RegisterResult FailedRegistrationResult()
        {
            _logger.LogInformation("Account registration failed.");
            return new RegisterResult(false, "Operation failed", -1, -1, -1);
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

            var authToken = string.Empty; // Todo : Implement token generation service

            _logger.LogDebug("User {Username} signed in", request.Username);

            return new SignInResult(true, string.Empty, authToken);
        }
    }
}