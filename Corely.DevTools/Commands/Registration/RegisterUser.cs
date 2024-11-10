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
        internal class RegisterUser : CommandBase
        {
            [Argument("Filepath to register user request json", true)]
            private string RequestJsonFile { get; init; } = null!;

            [Option("-c", "--create", Description = "Create sample json file at path")]
            private bool Create { get; init; }

            private readonly IRegistrationService _registrationService;

            public RegisterUser(IRegistrationService registrationService) : base("user", "Register a new user")
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
                    await RegisterUserAsync();
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

                var registerRequest = new RegisterUserRequest("userName", "email", "password");
                var json = JsonSerializer.Serialize(registerRequest);
                File.WriteAllText(RequestJsonFile, json);

                Console.WriteLine($"Sample json file created at: {RequestJsonFile}");
            }

            private async Task RegisterUserAsync()
            {

                if (!File.Exists(RequestJsonFile))
                {
                    Console.WriteLine($"File not found: {RequestJsonFile}");
                    return;
                }

                var json = File.ReadAllText(RequestJsonFile);

                var registerRequest = JsonSerializer.Deserialize<RegisterUserRequest>(json);

                if (registerRequest == null)
                {
                    Console.WriteLine($"Invalid json: {RequestJsonFile}");
                    return;
                }

                try
                {
                    var result = await _registrationService.RegisterUserAsync(registerRequest);

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
