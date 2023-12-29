using AutoMapper;

namespace Corely.Domain.Mappers.AutoMapper
{
    public class AutoMapperMapProvider(IMapper mapper) : IMapProvider
    {
        private readonly IMapper _mapper = mapper;

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}
