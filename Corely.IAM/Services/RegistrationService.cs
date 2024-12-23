using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Accounts.Processors;
using Corely.IAM.BasicAuths.Models;
using Corely.IAM.BasicAuths.Processors;
using Corely.IAM.Groups.Models;
using Corely.IAM.Groups.Processors;
using Corely.IAM.Models;
using Corely.IAM.Roles.Models;
using Corely.IAM.Roles.Processors;
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
    private readonly IRoleProcessor _roleProcessor;
    private readonly IUnitOfWorkProvider _uowProvider;

    public RegistrationService(
        ILogger<RegistrationService> logger,
        IAccountProcessor accountProcessor,
        IUserProcessor userProcessor,
        IBasicAuthProcessor basicAuthProcessor,
        IGroupProcessor groupProcessor,
        IRoleProcessor roleProcessor,
        IUnitOfWorkProvider uowProvider)
    {
        _logger = logger.ThrowIfNull(nameof(logger));
        _accountProcessor = accountProcessor.ThrowIfNull(nameof(accountProcessor));
        _userProcessor = userProcessor.ThrowIfNull(nameof(userProcessor));
        _basicAuthProcessor = basicAuthProcessor.ThrowIfNull(nameof(basicAuthProcessor));
        _groupProcessor = groupProcessor.ThrowIfNull(nameof(groupProcessor));
        _roleProcessor = roleProcessor.ThrowIfNull(nameof(roleProcessor));
        _uowProvider = uowProvider.ThrowIfNull(nameof(uowProvider));
    }

    public async Task<RegisterUserResult> RegisterUserAsync(RegisterUserRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _logger.LogInformation("Registering user {User}", request.Username);

        bool uowSucceeded = false;
        try
        {
            await _uowProvider.BeginAsync();

            var userResult = await _userProcessor.CreateUserAsync(new(request.Username, request.Email));
            if (userResult.ResultCode != CreateUserResultCode.Success)
            {
                _logger.LogInformation("Registering user failed for username {Username}", request.Username);
                return new RegisterUserResult(RegisterUserResultCode.UserCreationError, userResult.Message, -1, -1);
            }

            var basicAuthResult = await _basicAuthProcessor.UpsertBasicAuthAsync(new(userResult.CreatedId, request.Password));
            if (basicAuthResult.ResultCode != UpsertBasicAuthResultCode.Success)
            {
                _logger.LogInformation("Registering basic auth failed for username {Username}", request.Username);
                return new RegisterUserResult(RegisterUserResultCode.BasicAuthCreationError, basicAuthResult.Message, -1, -1);
            }

            await _uowProvider.CommitAsync();
            uowSucceeded = true;
            _logger.LogInformation("User {Username} registered with Id {UserId}", request.Username, userResult.CreatedId);
            return new RegisterUserResult(RegisterUserResultCode.Success, string.Empty, userResult.CreatedId, basicAuthResult.CreatedId);
        }
        finally
        {
            if (!uowSucceeded)
            {
                await _uowProvider.RollbackAsync();
            }
        }
    }

    public async Task<RegisterAccountResult> RegisterAccountAsync(RegisterAccountRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _logger.LogInformation("Registering account {AccountName}", request.AccountName);

        var uowSucceeded = false;
        try
        {
            await _uowProvider.BeginAsync();

            var result = await _accountProcessor.CreateAccountAsync(new(request.AccountName, request.OwnerUserId));
            if (result.ResultCode != CreateAccountResultCode.Success)
            {
                _logger.LogInformation("Registering account failed for account name {AccountName}", request.AccountName);
                return new RegisterAccountResult(result.ResultCode, result.Message, -1);
            }

            await _roleProcessor.CreateDefaultSystemRolesAsync(result.CreatedId);

            await _uowProvider.CommitAsync();
            uowSucceeded = true;
            _logger.LogInformation("Account {AccountName} registered with Id {AccountId}", request.AccountName, result.CreatedId);
            return new RegisterAccountResult(result.ResultCode, string.Empty, result.CreatedId);
        }
        finally
        {
            if (!uowSucceeded)
            {
                await _uowProvider.RollbackAsync();
            }
        }
    }

    public async Task<RegisterGroupResult> RegisterGroupAsync(RegisterGroupRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _logger.LogInformation("Registering group {GroupName}", request.GroupName);

        var result = await _groupProcessor.CreateGroupAsync(new(request.GroupName, request.OwnerAccountId));
        if (result.ResultCode != CreateGroupResultCode.Success)
        {
            _logger.LogInformation("Registering group failed for group name {GroupName}", request.GroupName);
            return new RegisterGroupResult(result.ResultCode, result.Message, -1);
        }

        _logger.LogInformation("Group {GroupName} registered with Id {GroupId}", request.GroupName, result.CreatedId);

        return new RegisterGroupResult(result.ResultCode, string.Empty, result.CreatedId);
    }

    public async Task<RegisterRoleResult> RegisterRoleAsync(RegisterRoleRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _logger.LogInformation("Registering role {RoleName}", request.RoleName);

        var result = await _roleProcessor.CreateRoleAsync(new(request.RoleName, request.OwnerAccountId));
        if (result.ResultCode != CreateRoleResultCode.Success)
        {
            _logger.LogInformation("Registering role failed for role name {RoleName}", request.RoleName);
            return new RegisterRoleResult(result.ResultCode, result.Message, -1);
        }

        _logger.LogInformation("Role {RoleName} registered with Id {RoleId}", request.RoleName, result.CreatedId);

        return new RegisterRoleResult(result.ResultCode, string.Empty, result.CreatedId);
    }

    public async Task<RegisterUsersWithGroupResult> RegisterUsersWithGroupAsync(RegisterUsersWithGroupRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _logger.LogInformation("Registering user ids {@UserIds} with group id {GroupId}", request.UserIds, request.GroupId);

        var result = await _groupProcessor.AddUsersToGroupAsync(new(request.UserIds, request.GroupId));
        if (result.ResultCode != AddUsersToGroupResultCode.Success
            && result.ResultCode != AddUsersToGroupResultCode.PartialSuccess)
        {
            _logger.LogInformation("Registering users with group failed for group id {GroupId}", request.GroupId);
            return new RegisterUsersWithGroupResult(
                result.ResultCode,
                result.Message ?? string.Empty,
                result.AddedUserCount,
                result.InvalidUserIds);
        }

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["@InvalidUserIds"] = result.InvalidUserIds
        }))
        {
            _logger.LogInformation("Registered {RegisteredUserCount} users with group id {GroupId}", result.AddedUserCount, request.GroupId);
        }

        return new RegisterUsersWithGroupResult(
            result.ResultCode,
            string.Empty,
            request.UserIds.Count,
            result.InvalidUserIds);
    }

    public async Task<RegisterRolesWithGroupResult> RegisterRolesWithGroupAsync(RegisterRolesWithGroupRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _logger.LogInformation("Registering role ids {@RoleIds} with group id {GroupId}", request.RoleIds, request.GroupId);

        var result = await _groupProcessor.AssignRolesToGroupAsync(new(request.RoleIds, request.GroupId));
        if (result.ResultCode != AssignRolesToGroupResultCode.Success
            && result.ResultCode != AssignRolesToGroupResultCode.PartialSuccess)
        {
            _logger.LogInformation("Registering roles with group failed for group id {GroupId}", request.GroupId);
            return new RegisterRolesWithGroupResult(
                result.ResultCode,
                result.Message ?? string.Empty,
                result.AddedRoleCount,
                result.InvalidRoleIds);
        }

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["@InvalidRoleIds"] = result.InvalidRoleIds
        }))
        {
            _logger.LogInformation("Registered {RegisteredRoleCount} roles with group id {GroupId}", result.AddedRoleCount, request.GroupId);
        }

        return new RegisterRolesWithGroupResult(
            result.ResultCode,
            string.Empty,
            request.RoleIds.Count,
            result.InvalidRoleIds);
    }

    public async Task<RegisterRolesWithUserResult> RegisterRolesWithUserAsync(RegisterRolesWithUserRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _logger.LogInformation("Registering role ids {@RoleIds} with user id {UserId}", request.RoleIds, request.UserId);

        var result = await _userProcessor.AssignRolesToUserAsync(new(request.RoleIds, request.UserId));
        if (result.ResultCode != AssignRolesToUserResultCode.Success
            && result.ResultCode != AssignRolesToUserResultCode.PartialSuccess)
        {
            _logger.LogInformation("Registering roles with user failed for user id {UserId}", request.UserId);
            return new RegisterRolesWithUserResult(
                result.ResultCode,
                result.Message ?? string.Empty,
                result.AddedRoleCount,
                result.InvalidRoleIds);
        }

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["@InvalidRoleIds"] = result.InvalidRoleIds
        }))
        {
            _logger.LogInformation("Registered {RegisteredRoleCount} roles with user id {UserId}", result.AddedRoleCount, request.UserId);
        }

        return new RegisterRolesWithUserResult(
            result.ResultCode,
            string.Empty,
            request.RoleIds.Count,
            result.InvalidRoleIds);
    }
}
