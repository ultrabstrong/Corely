﻿using Corely.Common.Extensions;
using Corely.DevTools.Attributes;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Corely.IAM.Validators;
using System.Text.Json;

namespace Corely.DevTools.Commands.Registration;

internal partial class Registration : CommandBase
{
    internal class RegisterRolesWithUser : CommandBase
    {
        [Argument("Filepath to register roles with user request json", true)]
        private string RequestJsonFile { get; init; } = null!;

        [Option("-c", "--create", Description = "Create sample json file at path")]
        private bool Create { get; init; }

        private readonly IRegistrationService _registrationService;

        public RegisterRolesWithUser(IRegistrationService registrationService) : base("roles-with-user", "Register roles with user")
        {
            _registrationService = registrationService.ThrowIfNull(nameof(registrationService));
        }

        protected async override Task ExecuteAsync()
        {
            if (Create)
            {
                CreateSampleJson(RequestJsonFile, new RegisterRolesWithUserRequest([1, 2], 3));
            }
            else
            {
                await RegisterRolesWithUserAsync();
            }
        }

        private async Task RegisterRolesWithUserAsync()
        {
            var request = ReadRequestJson<RegisterRolesWithUserRequest>(RequestJsonFile);
            if (request == null) return;
            try
            {
                foreach (var registerRequest in request)
                {
                    var result = await _registrationService.RegisterRolesWithUserAsync(registerRequest);
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
