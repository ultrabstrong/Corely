using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Hashing;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Mappers;
using Corely.Domain.Models.Auth;
using Serilog;
using Serilog.Core;

namespace ConsoleTest
{
    internal class Program
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
#pragma warning restore IDE0052 // Remove unread private members
        private static readonly Logger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

        static void Main()
        {
            try
            {
                using var serviceFactory = new ServiceFactory();
                var mapper = serviceFactory.GetRequiredService<IMapProvider>();
                var hashProvider = serviceFactory.GetRequiredService<IHashProvider>();

                var originalBasicAuth = new BasicAuth()
                {
                    ModifiedUtc = DateTime.UtcNow,
                    Password = new HashedValue(hashProvider).Set("password"),
                    Username = "username"
                };

                var mappedBasicAuthEntity = mapper.Map<BasicAuthEntity>(originalBasicAuth);
                var mappedBasicAuth = mapper.Map<BasicAuth>(mappedBasicAuthEntity);
                var isVerified = mappedBasicAuth.Password.Verify("password");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred");
            }
            logger.Information("Program finished. Press any key to exit.");
            Console.ReadKey();
        }
    }
}