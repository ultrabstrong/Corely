using AutoMapper;

namespace Corely.Domain.Mappers.AutoMapper
{
    public class AutoMapperMapProvider(IMapper mapper) : IMapProvider
    {
        public TDestination Map<TDestination>(object source)
        {
            return mapper.Map<TDestination>(source);
        }
    }
}
