﻿using ConsoleTest.SerilogCustomization;
using Corely.Common.Providers.Redaction;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            .Enrich.WithProperty("Application", "ConsoleTest")
            .Enrich.WithProperty("CorrelationId", Guid.NewGuid())
            .Enrich.With(new SerilogRedactionEnricher([
                new PasswordRedactionProvider()]))
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();

        try
        {
            using var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    ServiceFactory.Instance.AddIAMServices(services);
                })
                .Build();

            var registrationService = host.Services.GetRequiredService<IRegistrationService>();

            var registerUserRequest = new RegisterUserRequest("un1", "email@x.y", "admin");
            var registerUserResult = await registrationService.RegisterUserAsync(registerUserRequest);

            var registerAccountRequest = new RegisterAccountRequest("acct1", registerUserResult.CreatedUserId);
            var registerAccountResult = await registrationService.RegisterAccountAsync(registerAccountRequest);

            var registerGroupRequest = new RegisterGroupRequest("grp1", registerAccountResult.CreatedAccountId);
            var registerGroupResult = await registrationService.RegisterGroupAsync(registerGroupRequest);

            var registerUsersWithGroupRequest = new RegisterUsersWithGroupRequest([registerUserResult.CreatedUserId, 9999, 8888], registerGroupResult.CreatedGroupId);
            var registerUsersWithGroupResult = await registrationService.RegisterUsersWithGroupAsync(registerUsersWithGroupRequest);

            var signInService = host.Services.GetRequiredService<ISignInService>();
            var signInRequest = new SignInRequest("un1", "admin");
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
