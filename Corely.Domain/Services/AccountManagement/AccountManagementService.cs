using Corely.Common.Extensions;
using Corely.Domain.Mappers;
using Corely.Domain.Models.AccountManagement;
using Corely.Domain.Services.Accounts;
using Corely.Domain.Services.Auth;
using Corely.Domain.Services.Users;
using Corely.Domain.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.Domain.Services.AccountManagement
{
    public class AccountManagementService : ServiceBase, IAccountManagementService
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AccountManagementService(
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger logger,
            IAccountService accountService,
            IUserService userService,
            IAuthService authService)
            : base(mapProvider, validationProvider, logger)
        {
            _accountService = accountService.ThrowIfNull(nameof(accountService));
            _userService = userService.ThrowIfNull(nameof(userService));
            _authService = authService.ThrowIfNull(nameof(authService));
        }

        public async Task<SignUpResult> SignUpAsync(SignUpRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            logger.LogInformation("Signing up account {Account}", request.AccountName);

            var createAccountResult = await _accountService.CreateAccountAsync(
                new(request.AccountName));
            if (createAccountResult.IsSuccess)
            {
                var createUserResult = await _userService.CreateUserAsync(
                    new(createAccountResult.CreatedId, request.Username, request.Email));
                if (createUserResult.IsSuccess)
                {
                    var createAuthResult = await _authService.UpsertBasicAuthAsync(
                        new(createUserResult.CreatedId, request.Username, request.Password));
                    if (createAuthResult.IsSuccess)
                    {
                        logger.LogInformation("Account {Account} signed up with Id {Id}", request.AccountName, createAccountResult.CreatedId);
                        return new SignUpResult(true, "",
                            createAccountResult.CreatedId,
                            createUserResult.CreatedId,
                            createAuthResult.CreatedId);
                    }
                }
            }

            logger.LogInformation("Account {Account} sign up failed", request.AccountName);
            return new SignUpResult(false, "", -1, -1, -1);
        }
    }
}
