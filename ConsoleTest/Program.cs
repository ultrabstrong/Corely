using Corely.Common.Providers.Redaction;
using Corely.Domain.Models.AccountManagement;
using Corely.Domain.Services.AccountManagement;
using Serilog;

namespace ConsoleTest
{
    internal class Program
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
#pragma warning restore IDE0052 // Remove unread private members

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

                var signUpRequest = new RegisterRequest("acct1", "un1", "email@x.y", "password");

                var acctMgmtService = serviceFactory.GetRequiredService<IAccountManagementService>();
                var signUpResult = await acctMgmtService.RegisterAsync(signUpRequest);
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
