using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Keys;
using Corely.Domain.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Corely.UnitTests
{
    internal class ServiceFactory : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;

        public ServiceFactory()
        {

            var services = new ServiceCollection();

            services.AddAutoMapper(typeof(IMapProvider).Assembly);
            AddSecurityServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void AddSecurityServices(IServiceCollection services)
        {
            var key = new AesKeyProvider().CreateKey();
            services.AddScoped<IKeyStoreProvider, InMemoryKeyStoreProvider>(_ =>
                new InMemoryKeyStoreProvider(key));

            services.AddScoped<IEncryptionProviderFactory, EncryptionProviderFactory>();
            services.AddScoped<IHashProviderFactory, HashProviderFactory>();
        }

        public T GetRequiredService<T>() where T : notnull
            => _serviceProvider.GetRequiredService<T>();

        public void Dispose() => _serviceProvider?.Dispose();
    }
}
