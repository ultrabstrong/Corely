using Corely.Common.Models.Security;
using Corely.Domain;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Mappers;
using Corely.Domain.Models.Auth;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace ConsoleTest
{
    internal class Program
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        private static readonly Logger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
#pragma warning restore IDE0052 // Remove unread private members

        static void Main()
        {
            try
            {
                var serviceProvider = CreateServiceProvider();
                var mapper = serviceProvider.GetRequiredService<IMapProvider>();
                mapper.Map<BasicAuthEntity>(new BasicAuth()
                {
                    ModifiedUtc = DateTime.UtcNow,
                    Password = new HashedValue("password"),
                    Username = "username"
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred");
            }
            logger.Information("Program finished. Press any key to exit.");
            Console.ReadKey();
        }

        private static ServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddSerilog(logger));
            services.AddDomainServices();
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}