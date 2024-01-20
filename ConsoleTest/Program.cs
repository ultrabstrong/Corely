using Corely.Domain.Services.Accounts;
using Corely.Domain.Services.Auth;
using Corely.Domain.Services.Users;
using Serilog;

namespace ConsoleTest
{
    internal class Program
    {
#pragma warning disable IDE0052 // Remove unused private members
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
#pragma warning restore IDE0052 // Remove unused private members


        static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "ConsoleTest")
                .Enrich.WithProperty("CorrelationId", Guid.NewGuid())
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            try
            {
                using var serviceFactory = new ServiceFactory();

                var accountService = serviceFactory.GetRequiredService<IAccountService>();
                var accountName = "1232";
                await accountService.CreateAccountAsync(new(accountName));

                var userService = serviceFactory.GetRequiredService<IUserService>();
                var username = "bstrong";
                var email = "ultrabstrong";
                var createResult = await userService.CreateUserAsync(new(username, email));
                await userService.CreateUserAsync(new(username, email));

                var authService = serviceFactory.GetRequiredService<IAuthService>();
                var password = "asdf";
                await authService.UpsertBasicAuthAsync(new(createResult.CreatedId, username, password));
                await authService.UpsertBasicAuthAsync(new(createResult.CreatedId, username, password));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "An error occurred");
            }
            Log.CloseAndFlush();
            Log.Logger.Information("Program finished.");
        }
    }
}
