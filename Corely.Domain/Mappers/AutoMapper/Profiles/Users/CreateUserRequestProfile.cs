using AutoMapper;
using Corely.Domain.Models.Users;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Users
{
    internal class CreateUserRequestProfile : Profile
    {
        public CreateUserRequestProfile()
        {
            CreateMap<CreateUserRequest, User>(MemberList.Source)
                .ForSourceMember(m => m.AccountId, opt => opt.DoNotValidate());
        }
    }
}
