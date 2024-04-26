using Corely.Common.Extensions;
using Corely.IAM.Accounts.Services;
using Corely.IAM.Auth.Services;
using Corely.IAM.Models.AccountManagement;
using Corely.IAM.Repos;
using Corely.IAM.Users.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Services.AccountManagement
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
                return new RegisterResult(true, "", createAccountResult.CreatedId, createUserResult.CreatedId, createAuthResult.CreatedId);
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
    }
}