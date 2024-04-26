using Corely.DevTools.Attributes;
using Corely.IAM.Models.AccountManagement;
using Corely.IAM.Services.AccountManagement;
using System.Text.Json;

namespace Corely.DevTools.Commands.AccountManagement
{
    internal partial class AccountManagement : CommandBase
    {
        internal class SignUp : CommandBase
        {
            [Argument("Filepath to signup request json", true)]
            private string JsonSignupRequestFile { get; init; } = null!;

            [Option("-c", "--create", Description = "Create sample json file at path")]
            private bool Create { get; init; }

            private readonly IAccountManagementService _accountManagementService;

            public SignUp() : base("signup", "Sign up for a new account")
            {
                var serviceFactory = new ServiceFactory();
                _accountManagementService = serviceFactory.GetRequiredService<IAccountManagementService>();
            }

            protected async override Task ExecuteAsync()
            {
                if (Create)
                {
                    CreateSampleJson();
                }
                else
                {
                    await SignUpUserAsync();
                }
            }

            private void CreateSampleJson()
            {
                FileInfo file = new(JsonSignupRequestFile);

                if (!Directory.Exists(file.DirectoryName))
                {
                    Console.WriteLine($"Directory not found: {file.DirectoryName}");
                    return;
                }

                var signUpRequest = new RegisterRequest("accountName", "userName", "email", "password");
                var json = JsonSerializer.Serialize(signUpRequest);
                File.WriteAllText(JsonSignupRequestFile, json);

                Console.WriteLine($"Sample json file created at: {JsonSignupRequestFile}");
            }

            private async Task SignUpUserAsync()
            {
                if (!File.Exists(JsonSignupRequestFile))
                {
                    Console.WriteLine($"File not found: {JsonSignupRequestFile}");
                    return;
                }

                var json = File.ReadAllText(JsonSignupRequestFile);

                var signUpRequest = JsonSerializer.Deserialize<RegisterRequest>(json);

                if (signUpRequest == null)
                {
                    Console.WriteLine($"Invalid json: {JsonSignupRequestFile}");
                    return;
                }

                var result = await _accountManagementService.RegisterAsync(signUpRequest);

                Console.WriteLine(JsonSerializer.Serialize(result));
            }
        }
    }
}
