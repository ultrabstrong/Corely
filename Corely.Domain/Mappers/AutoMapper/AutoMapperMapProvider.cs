using AutoMapper;

namespace Corely.Domain.Mappers.AutoMapper
{
    public class AutoMapperMapProvider : IMapProvider
    {
        private readonly IMapper _mapper;

        public AutoMapperMapProvider(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}
