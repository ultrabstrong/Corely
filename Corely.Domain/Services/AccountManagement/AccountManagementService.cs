using Corely.Common.Extensions;
using Corely.Domain.Models.AccountManagement;
using Corely.Domain.Repos;
using Corely.Domain.Services.Accounts;
using Corely.Domain.Services.Auth;
using Corely.Domain.Services.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.Domain.Services.AccountManagement
{
    public class AccountManagementService : IAccountManagementService
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
            [FromKeyedServices(nameof(AccountManagementService))]
            IUnitOfWorkProvider uowProvider)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _accountService = accountService.ThrowIfNull(nameof(accountService));
            _userService = userService.ThrowIfNull(nameof(userService));
            _authService = authService.ThrowIfNull(nameof(authService));
            _uowProvider = uowProvider.ThrowIfNull(nameof(uowProvider));
        }

        public async Task<SignUpResult> SignUpAsync(SignUpRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            _logger.LogInformation("Signing up account {Account}", request.AccountName);

            bool operationSucceeded = false;
            try
            {
                await _uowProvider.BeginAsync();
                var createAccountResult = await _accountService.CreateAccountAsync(new(request.AccountName));
                if (!createAccountResult.IsSuccess)
                {
                    _logger.LogInformation("Creating account failed for {Account}", request.AccountName);
                    return FailSignUpResult();
                }

                var createUserResult = await _userService.CreateUserAsync(new(createAccountResult.AccountEntity, request.Username, request.Email));
                if (!createUserResult.IsSuccess)
                {
                    _logger.LogInformation("Creating user failed for account {Account}", request.AccountName);
                    return FailSignUpResult();
                }

                var createAuthResult = await _authService.UpsertBasicAuthAsync(new(createUserResult.CreatedId, request.Username, request.Password));
                if (!createAuthResult.IsSuccess)
                {
                    _logger.LogInformation("Creating auth failed for user {Username}", request.Username);
                    return FailSignUpResult();
                }

                await _uowProvider.CommitAsync();
                operationSucceeded = true;
                _logger.LogInformation("Account {Account} signed up with Id {Id}", request.AccountName, createAccountResult.CreatedId);
                return new SignUpResult(true, "", createAccountResult.CreatedId, createUserResult.CreatedId, createAuthResult.CreatedId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error signing up account {Account}", request.AccountName);
                return new SignUpResult(false, ex.Message, -1, -1, -1);
            }
            finally
            {
                if (!operationSucceeded)
                {
                    await _uowProvider.RollbackAsync();
                }
            }
            // BHS writing this from Peurto Morelos, 2024!
        }

        private SignUpResult FailSignUpResult()
        {
            _logger.LogInformation("Sign up operation failed.");
            return new SignUpResult(false, "Operation failed", -1, -1, -1);
        }
    }
}