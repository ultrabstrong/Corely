using AutoMapper;

namespace Corely.IAM.Mappers.AutoMapper
{
    internal sealed class AutoMapperMapProvider : IMapProvider
    {
        private readonly IMapper _mapper;

        public AutoMapperMapProvider(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination MapTo<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}
