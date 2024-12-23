﻿using Corely.Common.Extensions;
using Corely.DevTools.Attributes;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Corely.IAM.Validators;
using System.Text.Json;

namespace Corely.DevTools.Commands.Registration;
internal partial class Registration : CommandBase
{
    internal class RegisterRole : CommandBase
    {
        [Argument("Filepath to register role request json", true)]
        private string RequestJsonFile { get; init; } = null!;

        [Option("-c", "--create", Description = "Create sample json file at path")]
        private bool Create { get; init; }

        private readonly IRegistrationService _registrationService;

        public RegisterRole(IRegistrationService registrationService) : base("role", "Register a new role")
        {
            _registrationService = registrationService.ThrowIfNull(nameof(registrationService));
        }

        protected async override Task ExecuteAsync()
        {
            if (Create)
            {
                CreateSampleJson(RequestJsonFile, new RegisterRoleRequest("roleName", 1));
            }
            else
            {
                await RegisterRoleAsync();
            }
        }

        private async Task RegisterRoleAsync()
        {
            var request = ReadRequestJson<RegisterRoleRequest>(RequestJsonFile);
            if (request == null) return;
            try
            {
                foreach (var registerRequest in request)
                {
                    var result = await _registrationService.RegisterRoleAsync(registerRequest);
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
