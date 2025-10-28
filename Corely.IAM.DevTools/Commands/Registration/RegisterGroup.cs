﻿using Corely.Common.Extensions;
using Corely.IAM.DevTools.Attributes;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Corely.IAM.Validators;
using System.Text.Json;

namespace Corely.IAM.DevTools.Commands.Registration;


internal partial class Registration : CommandBase
{
    internal class RegisterGroup : CommandBase
    {
        [Argument("Filepath to register group request json", true)]
        private string RequestJsonFile { get; init; } = null!;

        [Option("-c", "--create", Description = "Create sample json file at path")]
        private bool Create { get; init; }

        private readonly IRegistrationService _registrationService;

        public RegisterGroup(IRegistrationService registrationService) : base("group", "Register a new group")
        {
            _registrationService = registrationService.ThrowIfNull(nameof(registrationService));
        }

        protected async override Task ExecuteAsync()
        {
            if (Create)
            {
                CreateSampleJson(RequestJsonFile, new RegisterGroupRequest("groupName", 1));
            }
            else
            {
                await RegisterGroupAsync();
            }
        }

        private async Task RegisterGroupAsync()
        {
            var request = ReadRequestJson<RegisterGroupRequest>(RequestJsonFile);
            if (request == null) return;

            try
            {
                foreach (var registerRequest in request)
                {
                    var result = await _registrationService.RegisterGroupAsync(registerRequest);
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
