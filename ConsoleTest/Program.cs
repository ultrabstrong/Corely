using ConsoleTest.SerilogCustomization;
using Corely.Common.Providers.Redaction;
using Corely.Common.Text.Delimited;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;

namespace ConsoleTest;

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
            .Enrich.WithProperty("Application", nameof(ConsoleTest))
            .Enrich.WithProperty("CorrelationId", Guid.NewGuid())
            .Enrich.With(new SerilogRedactionEnricher([
                new PasswordRedactionProvider()]))
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();

        try
        {
            long lastRecordEndPosition = 0; // Get the last successful record end position
            var provider = new DelimitedTextProvider(NullLogger<DelimitedTextProvider>.Instance);
            using (var stream = File.OpenRead("data.csv"))
            {
                var record = new ReadRecordResult() { StartPosition = lastRecordEndPosition };
                while (record.HasMore)
                {
                    record = provider.ReadNextRecord(stream, record.EndPosition);
                    // Handle record
                    // Save the end position of this record in case next record read is interrupted
                }
            }
            return;
            using var host = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    new ServiceFactory(services, hostContext.Configuration).AddIAMServices();
                })
                .Build();

            var registrationService = host.Services.GetRequiredService<IRegistrationService>();

            var registerUserRequest = new RegisterUserRequest("user1", "email@x.y", "admin");
            var registerUserResult = await registrationService.RegisterUserAsync(registerUserRequest);

            var registerAccountRequest = new RegisterAccountRequest("acct1", registerUserResult.CreatedUserId);
            var registerAccountResult = await registrationService.RegisterAccountAsync(registerAccountRequest);

            var registerGroupRequest = new RegisterGroupRequest("grp1", registerAccountResult.CreatedAccountId);
            var registerGroupResult = await registrationService.RegisterGroupAsync(registerGroupRequest);

            var registerPermissionRequest = new RegisterPermissionRequest("perm1", registerAccountResult.CreatedAccountId, "group", registerGroupResult.CreatedGroupId);
            var registerPermissionResult = await registrationService.RegisterPermissionAsync(registerPermissionRequest);

            var registerUsersWithGroupRequest = new RegisterUsersWithGroupRequest([registerUserResult.CreatedUserId, 9999, 8888], registerGroupResult.CreatedGroupId);
            var registerUsersWithGroupResult = await registrationService.RegisterUsersWithGroupAsync(registerUsersWithGroupRequest);

            var registerRoleRequest = new RegisterRoleRequest("role1", registerAccountResult.CreatedAccountId);
            var registerRoleResult = await registrationService.RegisterRoleAsync(registerRoleRequest);

            var registerPermissionsWithRoleRequest = new RegisterPermissionsWithRoleRequest([registerPermissionResult.CreatedPermissionId, 9999, 8888], registerRoleResult.CreatedRoleId);
            var registerPermissionsWithRoleResult = await registrationService.RegisterPermissionsWithRoleAsync(registerPermissionsWithRoleRequest);

            var signInService = host.Services.GetRequiredService<ISignInService>();
            var signInRequest = new SignInRequest("user1", "admin");
            var signInResult = await signInService.SignInAsync(signInRequest);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "An error occurred");
        }
        Log.CloseAndFlush();
        Log.Logger.Information("Program finished.");
    }
}
