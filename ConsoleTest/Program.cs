using Corely.Common.Providers.Redaction;
using Corely.Domain.Models.Auth;
using Serilog;
using System.Text.Json;

namespace ConsoleTest
{
    internal class Program
    {
#pragma warning disable IDE0052 // Remove unused private members
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
#pragma warning restore IDE0052 // Remove unused private members
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

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
                var basicAuthRequest = new UpsertBasicAuthRequest(1, "username", "as@#$%#$^   09u09a8s09fj;qo34\"808+_)(*&^%$@!$#@^");
                string json = JsonSerializer.Serialize(basicAuthRequest, jsonSerializerOptions);
                //json = @"{""UserId"":1,""Username"":""username"",""Password"":""as@#$%#$^   09u09a8s09fj;qo34\""808+_)(*&^%$@!$#@^"",""UserId"":1,""Username"":""username"",""Password"":""as@#$%#$^   09u09a8s09fj;qo34\""808+_)(*&^%$@!$#@^""}";

                Log.Logger.Information(json);

                var jsonContextLogger = Log.Logger
                    .ForContext("basicAuth", basicAuthRequest)
                    .ForContext("json", json);
                jsonContextLogger.Information("New json context logger");

                /*
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

                Console.WriteLine(JsonSerializer.Serialize(basicAuthRequest));


                await authService.UpsertBasicAuthAsync(new(createResult.CreatedId, username, password));
                await authService.UpsertBasicAuthAsync(new(createResult.CreatedId, username, password));
                */
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
