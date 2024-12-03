using AutoMapper;
using Corely.IAM.Groups.Models;

namespace Corely.IAM.Groups.Mappers;

internal class CreateGroupRequestProfile : Profile
{
    public CreateGroupRequestProfile()
    {
        CreateMap<CreateGroupRequest, Group>(MemberList.Source)
            .ForMember(m => m.AccountId, opt => opt.MapFrom(m => m.OwnerAccountId));
    }
}
