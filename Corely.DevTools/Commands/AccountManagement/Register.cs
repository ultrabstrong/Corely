﻿using Corely.DevTools.Attributes;
using Corely.IAM.AccountManagement.Models;
using Corely.IAM.AccountManagement.Services;
using System.Text.Json;

namespace Corely.DevTools.Commands.AccountManagement
{
    internal partial class AccountManagement : CommandBase
    {
        internal class Register : CommandBase
        {
            [Argument("Filepath to register request json", true)]
            private string JsonRegisterRequestFile { get; init; } = null!;

            [Option("-c", "--create", Description = "Create sample json file at path")]
            private bool Create { get; init; }

            private readonly IAccountManagementService _accountManagementService;

            public Register() : base("register", "Register for a new account")
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
                    await RegisterUserAsync();
                }
            }

            private void CreateSampleJson()
            {
                FileInfo file = new(JsonRegisterRequestFile);

                if (!Directory.Exists(file.DirectoryName))
                {
                    Console.WriteLine($"Directory not found: {file.DirectoryName}");
                    return;
                }

                var registerRequest = new RegisterRequest("accountName", "userName", "email", "password");
                var json = JsonSerializer.Serialize(registerRequest);
                File.WriteAllText(JsonRegisterRequestFile, json);

                Console.WriteLine($"Sample json file created at: {JsonRegisterRequestFile}");
            }

            private async Task RegisterUserAsync()
            {
                if (!File.Exists(JsonRegisterRequestFile))
                {
                    Console.WriteLine($"File not found: {JsonRegisterRequestFile}");
                    return;
                }

                var json = File.ReadAllText(JsonRegisterRequestFile);

                var registerRequest = JsonSerializer.Deserialize<RegisterRequest>(json);

                if (registerRequest == null)
                {
                    Console.WriteLine($"Invalid json: {JsonRegisterRequestFile}");
                    return;
                }

                var result = await _accountManagementService.RegisterAsync(registerRequest);

                Console.WriteLine(JsonSerializer.Serialize(result));
            }
        }
    }
}
