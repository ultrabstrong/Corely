using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM.Accounts.Services;
using Corely.IAM.BasicAuths.Services;
using Corely.IAM.Models;
using Corely.IAM.Users.Services;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Services
{
    internal class RegistrationService : IRegistrationService
    {
        private readonly ILogger<RegistrationService> _logger;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IBasicAuthService _authService;
        private readonly IUnitOfWorkProvider _uowProvider;

        public RegistrationService(
            ILogger<RegistrationService> logger,
            IAccountService accountService,
            IUserService userService,
            IBasicAuthService authService,
            IUnitOfWorkProvider uowProvider)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _accountService = accountService.ThrowIfNull(nameof(accountService));
            _userService = userService.ThrowIfNull(nameof(userService));
            _authService = authService.ThrowIfNull(nameof(authService));
            _uowProvider = uowProvider.ThrowIfNull(nameof(uowProvider));
        }

        public async Task<RegisterUserResult> RegisterUserAsync(RegisterUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            _logger.LogInformation("Registering user {User}", request.Username);

            bool operationSucceeded = false;
            try
            {
                await _uowProvider.BeginAsync();

                var createUserResult = await _userService.CreateUserAsync(new(request.Username, request.Email));
                if (!createUserResult.IsSuccess)
                {
                    _logger.LogInformation("Creating user failed for user {User}", request.Username);
                    _logger.LogInformation("Account registration failed.");
                    return new RegisterUserResult(false, "Operation failed", -1, -1);
                }

                var createAuthResult = await _authService.UpsertBasicAuthAsync(new(createUserResult.CreatedId, request.Password));
                if (!createAuthResult.IsSuccess)
                {
                    _logger.LogInformation("Creating auth failed for user {Username}", request.Username);
                    _logger.LogInformation("Account registration failed.");
                    return new RegisterUserResult(false, "Operation failed", -1, -1);
                }

                await _uowProvider.CommitAsync();
                operationSucceeded = true;
                _logger.LogInformation("User {User} registered with Id {Id}", request.Username, createUserResult.CreatedId);
                return new RegisterUserResult(true, string.Empty, createUserResult.CreatedId, createAuthResult.CreatedId);
            }
            finally
            {
                if (!operationSucceeded)
                {
                    await _uowProvider.RollbackAsync();
                }
            }
        }

        public async Task<RegisterAccountResult> RegisterAccountAsync(RegisterAccountRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            _logger.LogInformation("Registering account {Account}", request.AccountName);

            bool operationSucceeded = false;
            try
            {
                await _uowProvider.BeginAsync();

                var createAccountResult = await _accountService.CreateAccountAsync(new(request.AccountName, request.OwnerUserId));
                if (!createAccountResult.IsSuccess)
                {
                    _logger.LogInformation("Creating account failed for account {Account}", request.AccountName);
                    _logger.LogInformation("Account registration failed.");
                    return new RegisterAccountResult(false, "Operation failed", -1);
                }

                await _uowProvider.CommitAsync();
                operationSucceeded = true;
                _logger.LogInformation("Account {Account} registered with Id {Id}", request.AccountName, createAccountResult.CreatedId);
                return new RegisterAccountResult(true, string.Empty, createAccountResult.CreatedId);
            }
            finally
            {
                if (!operationSucceeded)
                {
                    await _uowProvider.RollbackAsync();
                }
            }
        }
    }
}