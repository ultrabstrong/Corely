using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Groups.Processors;
using Corely.IAM.Mappers;
using Corely.IAM.Processors;
using Corely.IAM.Roles.Constants;
using Corely.IAM.Roles.Entities;
using Corely.IAM.Roles.Models;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Roles.Processors;
internal class RoleProcessor : ProcessorBase, IRoleProcessor
{
    private readonly IRepo<RoleEntity> _roleRepo;
    private readonly IReadonlyRepo<AccountEntity> _accountRepo;

    public RoleProcessor(
        IRepo<RoleEntity> roleRepo,
        IReadonlyRepo<AccountEntity> accountRepo,
        IMapProvider mapProvider,
        IValidationProvider validationProvider,
        ILogger<GroupProcessor> logger)
        : base(mapProvider, validationProvider, logger)
    {
        _roleRepo = roleRepo;
        _accountRepo = accountRepo;
    }

    public async Task<CreateRoleResult> CreateRoleAsync(CreateRoleRequest createRoleRequest)
    {
        return await LogRequestResultAspect(nameof(RoleProcessor), nameof(CreateRoleAsync), createRoleRequest, async () =>
        {
            var role = MapThenValidateTo<Role>(createRoleRequest);

            if (await _roleRepo.AnyAsync(r =>
                r.AccountId == role.AccountId && r.Name == role.Name))
            {
                Logger.LogWarning("Role with name {RoleName} already exists", role.Name);
                return new CreateRoleResult(CreateRoleResultCode.RoleExistsError, $"Role with name {role.Name} already exists", -1);
            }

            var accountEntity = await _accountRepo.GetAsync(role.AccountId);
            if (accountEntity == null)
            {
                Logger.LogWarning("Account with Id {AccountId} not found", role.AccountId);
                return new CreateRoleResult(CreateRoleResultCode.AccountNotFoundError, $"Account with Id {role.AccountId} not found", -1);
            }

            var roleEntity = MapTo<RoleEntity>(role)!; // role is validated 
            var createdId = await _roleRepo.CreateAsync(roleEntity);

            return new CreateRoleResult(CreateRoleResultCode.Success, string.Empty, createdId);
        });
    }

    public async Task CreateDefaultSystemRolesAsync(int ownerAccountId)
    {
        RoleEntity[] roleEntities =
            [
                new() {
                    AccountId = ownerAccountId,
                    Name = RoleConstants.OWNER_ROLE_NAME,
                    IsSystemDefined = true,
                },
                new() {
                    AccountId = ownerAccountId,
                    Name = RoleConstants.ADMIN_ROLE_NAME,
                    IsSystemDefined = true,
                },
                new() {
                    AccountId = ownerAccountId,
                    Name = RoleConstants.USER_ROLE_NAME,
                    IsSystemDefined = true,
                }
            ];

        await _roleRepo.CreateAsync(roleEntities);
    }

    public async Task<Role?> GetRoleAsync(int roleId)
    {
        return await LogRequestAspect(nameof(RoleProcessor), nameof(GetRoleAsync), roleId, async () =>
        {
            var roleEntity = await _roleRepo.GetAsync(roleId);

            if (roleEntity == null)
            {
                Logger.LogInformation("Role with Id {RoleId} not found", roleId);
                return null;
            }

            var role = MapTo<Role>(roleEntity);
            return role;
        });
    }

    public Task<Role?> GetRoleAsync(string roleName, int ownerAccountId)
    {
        return LogRequestAspect(nameof(RoleProcessor), nameof(GetRoleAsync), roleName, async () =>
        {
            var roleEntity = await _roleRepo.GetAsync(r =>
                r.Name == roleName && r.AccountId == ownerAccountId);

            if (roleEntity == null)
            {
                Logger.LogInformation("Role with name {RoleName} not found", roleName);
                return null;
            }

            var role = MapTo<Role>(roleEntity);
            return role;
        });
    }
}
