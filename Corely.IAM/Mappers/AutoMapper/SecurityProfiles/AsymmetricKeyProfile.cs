using AutoMapper;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Models;

namespace Corely.IAM.Mappers.AutoMapper.SecurityProfiles
{
    public class AsymmetricKeyProfile : Profile
    {
        public AsymmetricKeyProfile()
        {
            CreateMap<AsymmetricKey, AccountAsymmetricKeyEntity>()
                .ForMember(m => m.AccountId, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
