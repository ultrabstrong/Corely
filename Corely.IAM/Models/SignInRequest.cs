﻿namespace Corely.IAM.Models;

public record SignInRequest(
    string Username,
    string Password);
