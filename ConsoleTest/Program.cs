using Corely.Common.Providers.Redaction;
using Corely.Domain.Models.Auth;
using Corely.Domain.Services.Accounts;
using Corely.Domain.Services.Auth;
using Corely.Domain.Services.Users;
using Serilog;

namespace ConsoleTest
{
    internal class Program
    {
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";

        static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "ConsoleTest")
                .Enrich.WithProperty("CorrelationId", Guid.NewGuid())
                .Enrich.With(new RedactionEnricher([
                    new PasswordRedactionProvider()]))
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
                var email = "ultrabstrong@gmail.com";
                var createResult = await userService.CreateUserAsync(new(username, email));

                var authService = serviceFactory.GetRequiredService<IAuthService>();
                var password = "asdfsadf";
                var basicAuthRequest = new UpsertBasicAuthRequest(createResult.CreatedId, username, password);

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
