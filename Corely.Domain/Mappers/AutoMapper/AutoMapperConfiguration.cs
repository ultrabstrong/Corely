using AutoMapper;
using System.Reflection;

namespace Corely.Domain.Mappers.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration GetConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            return config;
        }
    }
}
