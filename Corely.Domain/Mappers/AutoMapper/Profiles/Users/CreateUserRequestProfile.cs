using AutoMapper;
using Corely.Domain.Models.Auth;
using Corely.Domain.Models.Users;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Users
{
    internal class CreateUserRequestProfile : Profile
    {
        public CreateUserRequestProfile()
        {
            CreateMap<CreateUserRequest, User>(MemberList.Source)
                .ForSourceMember(src => src.Password, opt => opt.DoNotValidate());

            CreateMap<CreateUserRequest, BasicAuth>(MemberList.Source)
                .ForSourceMember(src => src.Email, opt => opt.DoNotValidate());
        }
    }
}
