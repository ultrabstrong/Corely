using Corely.Domain.Services.Users;
using Serilog;
using Serilog.Core;

namespace ConsoleTest
{
    internal class Program
    {
#pragma warning disable IDE0052 // Remove unused private members
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
#pragma warning restore IDE0052 // Remove unused private members
        private static readonly Logger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

        static void Main()
        {
            try
            {
                using var serviceFactory = new ServiceFactory();
                var userService = serviceFactory.GetRequiredService<IUserService>();

                var username = "bstrong";
                var email = "ultrabstrong@gmail.com";

                userService.CreateUser(new(username, email));

                userService.CreateUser(new(username, email));
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