using Corely.IAM.Roles.Models;

namespace Corely.IAM.Roles.Processors;
internal interface IRoleProcessor
{
    Task<CreateRoleResult> CreateRoleAsync(CreateRoleRequest createRoleRequest);
    Task CreateDefaultSystemRolesAsync(int ownerAccountID);
}
