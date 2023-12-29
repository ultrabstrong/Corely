using AutoMapper;
using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Factories;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Common
{
    public class IHashedValueProfile : Profile
    {
        public IHashedValueProfile(IHashProviderFactory providerFactory)
        {
            CreateMap<IHashedValue, string>()
                .ForMember(dest => dest, opt => opt.MapFrom(src => src.Hash));

            CreateMap<string, IHashedValue>()
                .ConstructUsing((string hash) => new HashedValue(providerFactory.GetProviderToVerify(hash), hash));
        }
    }
}
