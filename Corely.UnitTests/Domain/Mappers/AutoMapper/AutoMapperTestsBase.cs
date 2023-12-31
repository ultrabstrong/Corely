using AutoFixture;
using AutoMapper;
using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Keys;
using Corely.Domain.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper
{
    public abstract class AutoMapperTestBase<TSource, TDestination>
        where TSource : class
    {
        protected readonly IMapper _mapper;

        protected AutoMapperTestBase()
        {
            var services = new ServiceCollection();

            services.AddAutoMapper(typeof(IMapProvider).Assembly);
            AddSecurityServices(services);

            _mapper = services.BuildServiceProvider()
                .GetRequiredService<IMapper>();
        }

        private static void AddSecurityServices(IServiceCollection services)
        {
            var key = new AesKeyProvider().CreateKey();
            services.AddScoped<IKeyStoreProvider, InMemoryKeyStoreProvider>(_ =>
                new InMemoryKeyStoreProvider(key));

            services.AddScoped<IEncryptionProviderFactory, EncryptionProviderFactory>();
            services.AddScoped<IHashProviderFactory, HashProviderFactory>();
        }

        [Fact]
        public void Map_ShouldMapSourceToDestination()
        {
            var source = GetSource();
            var modifiedSource = ApplySourceModifications(source);
            _mapper.Map<TDestination>(modifiedSource);
        }

        protected virtual TSource GetSource() => GetMock<TSource>();

        protected virtual TSource ApplySourceModifications(TSource source) => source;

        protected T GetMock<T>() where T : class
        {
            try
            {
                return new Mock<T>(GetSourceParams()).Object;
            }
            catch (Exception)
            {
                return new Fixture().Create<T>();
            }
        }
        protected virtual object[] GetSourceParams() => [];

    }
}
