﻿using Corely.Common.Extensions;
using Corely.IAM.DevTools.Attributes;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Corely.IAM.Validators;
using System.Text.Json;

namespace Corely.IAM.DevTools.Commands.Registration;

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

                CreateSampleJson(RequestJsonFile, new RegisterAccountRequest("accountName", 1));
            }
            else
            {
                await RegisterAccountAsync();
            }
        }

        private async Task RegisterAccountAsync()
        {
            var request = ReadRequestJson<RegisterAccountRequest>(RequestJsonFile);
            if (request == null) return;

            try
            {
                foreach (var registerRequest in request)
                {
                    var result = await _registrationService.RegisterAccountAsync(registerRequest);
                    Console.WriteLine(JsonSerializer.Serialize(result));
                }
            }
            catch (ValidationException ex)
            {
                Error(ex.ValidationResult!.Errors!.Select(e => e.Message));
            }
        }
    }
}
