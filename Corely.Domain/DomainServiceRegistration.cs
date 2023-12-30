using Corely.Common.Providers.Security.Factories;
using Corely.Domain.Mappers;
using Corely.Domain.Mappers.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Corely.Domain
{
    public static class DomainServiceRegistration
    {
        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IHashProviderFactory, HashProviderFactory>();
            services.AddScoped<IEncryptionProviderFactory, EncryptionProviderFactory>();

            services.AddAutoMapper(typeof(DomainServiceRegistration).Assembly);
            services.AddScoped<IMapProvider, AutoMapperMapProvider>();
            //services.AddValidatorsFromAssemblyContaining<UserValidator>(includeInternalTypes: true);

            //services.AddScoped<IUserService, UserService>();
        }
    }
}
