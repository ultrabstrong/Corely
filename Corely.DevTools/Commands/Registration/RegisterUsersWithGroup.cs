﻿using Corely.Common.Extensions;
using Corely.DevTools.Attributes;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Corely.IAM.Validators;
using System.Text.Json;

namespace Corely.DevTools.Commands.Registration;


internal partial class Registration : CommandBase
{
    internal class RegisterUsersWithGroup : CommandBase
    {
        [Argument("Filepath to register user with group request json", true)]
        private string RequestJsonFile { get; init; } = null!;

        [Option("-c", "--create", Description = "Create sample json file at path")]
        private bool Create { get; init; }

        private readonly IRegistrationService _registrationService;

        public RegisterUsersWithGroup(IRegistrationService registrationService) : base("users-with-group", "Register a new user with group")
        {
            _registrationService = registrationService.ThrowIfNull(nameof(registrationService));
        }

        protected async override Task ExecuteAsync()
        {
            if (Create)
            {
                CreateSampleJson(RequestJsonFile, new RegisterUsersWithGroupRequest([1, 2], 3));
            }
            else
            {
                await RegisterUserWithGroupAsync();
            }
        }

        private async Task RegisterUserWithGroupAsync()
        {
            var request = ReadRequestJson<RegisterUsersWithGroupRequest>(RequestJsonFile);
            if (request == null) return;
            try
            {
                foreach (var registerRequest in request)
                {
                    var result = await _registrationService.RegisterUsersWithGroupAsync(registerRequest);
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
