using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Accounts.Processors;
using Corely.IAM.BasicAuths.Models;
using Corely.IAM.BasicAuths.Processors;
using Corely.IAM.Groups.Enums;
using Corely.IAM.Groups.Models;
using Corely.IAM.Groups.Processors;
using Corely.IAM.Models;
using Corely.IAM.Users.Models;
using Corely.IAM.Users.Processors;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Services;

internal class RegistrationService : IRegistrationService
{
    private readonly ILogger<RegistrationService> _logger;
    private readonly IAccountProcessor _accountProcessor;
    private readonly IUserProcessor _userProcessor;
    private readonly IBasicAuthProcessor _basicAuthProcessor;
    private readonly IGroupProcessor _groupProcessor;
    private readonly IUnitOfWorkProvider _uowProvider;

    public RegistrationService(
        ILogger<RegistrationService> logger,
        IAccountProcessor accountProcessor,
        IUserProcessor userProcessor,
        IBasicAuthProcessor basicAuthProcessor,
        IGroupProcessor groupProcessor,
        IUnitOfWorkProvider uowProvider)
    {
        _logger = logger.ThrowIfNull(nameof(logger));
        _accountProcessor = accountProcessor.ThrowIfNull(nameof(accountProcessor));
        _userProcessor = userProcessor.ThrowIfNull(nameof(userProcessor));
        _basicAuthProcessor = basicAuthProcessor.ThrowIfNull(nameof(basicAuthProcessor));
        _groupProcessor = groupProcessor.ThrowIfNull(nameof(groupProcessor));
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

            var createUserResult = await _userProcessor.CreateUserAsync(new(request.Username, request.Email));
            if (createUserResult.ResultCode != CreateUserResultCode.Success)
            {
                _logger.LogInformation("Creating user failed for username {Username}", request.Username);
                _logger.LogInformation("Account registration failed.");
                return new RegisterUserResult(RegisterUserResultCode.UserCreationError, createUserResult.Message, -1, -1);
            }

            var createAuthResult = await _basicAuthProcessor.UpsertBasicAuthAsync(new(createUserResult.CreatedId, request.Password));
            if (createAuthResult.ResultCode != UpsertBasicAuthResultCode.Success)
            {
                _logger.LogInformation("Creating auth failed for username {Username}", request.Username);
                _logger.LogInformation("Account registration failed.");
                return new RegisterUserResult(RegisterUserResultCode.BasicAuthCreationError, createAuthResult.Message, -1, -1);
            }

            await _uowProvider.CommitAsync();
            operationSucceeded = true;
            _logger.LogInformation("User {Username} registered with Id {UserId}", request.Username, createUserResult.CreatedId);
            return new RegisterUserResult(RegisterUserResultCode.Success, string.Empty, createUserResult.CreatedId, createAuthResult.CreatedId);
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
        _logger.LogInformation("Registering account {AccountName}", request.AccountName);

        bool operationSucceeded = false;
        try
        {
            await _uowProvider.BeginAsync();

            var createAccountResult = await _accountProcessor.CreateAccountAsync(new(request.AccountName, request.OwnerUserId));
            if (createAccountResult.ResultCode != CreateAccountResultCode.Success)
            {
                _logger.LogInformation("Creating account failed for account name {AccountName}", request.AccountName);
                _logger.LogInformation("Account registration failed.");
                return new RegisterAccountResult(createAccountResult.ResultCode, createAccountResult.Message, -1);
            }

            await _uowProvider.CommitAsync();
            operationSucceeded = true;
            _logger.LogInformation("Account {AccountName} registered with Id {AccountId}", request.AccountName, createAccountResult.CreatedId);
            return new RegisterAccountResult(createAccountResult.ResultCode, string.Empty, createAccountResult.CreatedId);
        }
        finally
        {
            if (!operationSucceeded)
            {
                await _uowProvider.RollbackAsync();
            }
        }
    }

    public async Task<RegisterGroupResult> RegisterGroupAsync(RegisterGroupRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _logger.LogInformation("Registering group {GroupName}", request.GroupName);

        var createGroupResult = await _groupProcessor.CreateGroupAsync(new(request.GroupName, request.OwnerAccountId));
        if (createGroupResult.ResultCode != CreateGroupResultCode.Success)
        {
            _logger.LogInformation("Creating group failed for group name {GroupName}", request.GroupName);
            _logger.LogInformation("Group registration failed.");
            return new RegisterGroupResult(createGroupResult.ResultCode, createGroupResult.Message, -1);
        }

        _logger.LogInformation("Group {GroupName} registered with Id {GroupId}", request.GroupName, createGroupResult.CreatedId);

        return new RegisterGroupResult(createGroupResult.ResultCode, string.Empty, createGroupResult.CreatedId);
    }

    public async Task<RegisterUsersWithGroupResult> RegisterUsersWithGroupAsync(RegisterUsersWithGroupRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _logger.LogInformation("Registering user ids {@UserIds} with group id {GroupId}", request.UserIds, request.GroupId);

        var addUsersToGroupResult = await _groupProcessor.AddUsersToGroupAsync(new(request.UserIds, request.GroupId));
        if (addUsersToGroupResult.ResultCode != AddUsersToGroupResultCode.Success
            && addUsersToGroupResult.ResultCode != AddUsersToGroupResultCode.PartialSuccess)
        {
            _logger.LogInformation("Registering users with group failed for group id {GroupId}", request.GroupId);
            _logger.LogInformation("Registering users with group failed.");
            return new RegisterUsersWithGroupResult(
                addUsersToGroupResult.ResultCode,
                addUsersToGroupResult.Message ?? string.Empty,
                addUsersToGroupResult.AddedUserCount,
                addUsersToGroupResult.InvalidUserIds);
        }

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["@InvalidUserIds"] = addUsersToGroupResult.InvalidUserIds
        }))
        {
            _logger.LogInformation("Registered {RegisteredUserCount} users with group id {GroupId}", addUsersToGroupResult.AddedUserCount, request.GroupId);
        }

        return new RegisterUsersWithGroupResult(
            addUsersToGroupResult.ResultCode,
            string.Empty,
            request.UserIds.Count,
            addUsersToGroupResult.InvalidUserIds);
    }
}
