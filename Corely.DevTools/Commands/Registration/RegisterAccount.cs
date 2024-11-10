using Corely.Common.Extensions;
using Corely.DevTools.Attributes;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Corely.IAM.Validators;
using Corely.Security.Password;
using System.Text.Json;

namespace Corely.DevTools.Commands.Registration
{
    internal partial class Registration : CommandBase
    {
        internal class RegisterAccount : CommandBase
        {
            [Argument("Filepath to register account request json", true)]
            private string RequestJsonFile { get; init; } = null!;

            [Option("-c", "--create", Description = "Create sample json file at path")]
            private bool Create { get; init; }

            private readonly IRegistrationService _registrationService;

            public RegisterAccount(IRegistrationService registrationService) : base("account", "Register a new account")
            {
                _registrationService = registrationService.ThrowIfNull(nameof(registrationService));
            }

            protected async override Task ExecuteAsync()
            {
                if (Create)
                {
                    CreateSampleJson();
                }
                else
                {
                    await RegisterAccountAsync();
                }
            }

            private void CreateSampleJson()
            {
                FileInfo file = new(RequestJsonFile);

                if (!Directory.Exists(file.DirectoryName))
                {
                    Console.WriteLine($"Directory not found: {file.DirectoryName}");
                    return;
                }

                var registerRequest = new RegisterAccountRequest("accountName", 1);
                var json = JsonSerializer.Serialize(registerRequest);
                File.WriteAllText(RequestJsonFile, json);

                Console.WriteLine($"Sample json file created at: {RequestJsonFile}");
            }

            private async Task RegisterAccountAsync()
            {
                if (!File.Exists(RequestJsonFile))
                {
                    Console.WriteLine($"File not found: {RequestJsonFile}");
                    return;
                }

                var json = File.ReadAllText(RequestJsonFile);

                var registerRequest = JsonSerializer.Deserialize<RegisterAccountRequest>(json);

                if (registerRequest == null)
                {
                    Console.WriteLine($"Invalid json: {RequestJsonFile}");
                    return;
                }
                try
                {
                    var result = await _registrationService.RegisterAccountAsync(registerRequest);

                    Console.WriteLine(JsonSerializer.Serialize(result));
                }
                catch (ValidationException ex)
                {
                    Error(ex.ValidationResult!.Errors!.Select(e => e.Message));
                }
                catch (PasswordValidationException ex)
                {
                    Error(ex.PasswordValidationResult.ValidationFailures);
                }
            }
        }
    }
}
