using AutoMapper;
using Corely.IAM.Security;

namespace Corely.IAM.Mappers.AutoMapper.SecurityProfiles
{
    public class SymmetricKeyProfile : Profile
    {
        public SymmetricKeyProfile()
        {
            CreateMap<SymmetricKey, SymmetricKeyEntity>()
                .ReverseMap();
        }
    }
}
