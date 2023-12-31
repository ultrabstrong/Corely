using AutoMapper;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Models.Auth;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Auth
{
    public sealed class BasicAuthProfile : Profile
    {
        public BasicAuthProfile()
        {
            CreateMap<BasicAuth, BasicAuthEntity>()
                .ReverseMap();
        }
    }
}
