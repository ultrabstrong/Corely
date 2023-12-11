using AutoMapper;
using System.Reflection;

namespace Corely.Domain.Providers
{
    public static class AutomapperConfigurationProvider
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
