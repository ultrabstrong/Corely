using AutoMapper;
using Corely.IAM.Roles.Models;

namespace Corely.IAM.Roles.Mappers;
internal class CreateRoleRequestProfile : Profile
{
    public CreateRoleRequestProfile()
    {
        CreateMap<CreateRoleRequest, Role>(MemberList.Source)
            .ForMember(m => m.AccountId, opt => opt.MapFrom(m => m.OwnerAccountId));
    }
}
