﻿namespace Corely.IAM.Security.Models;

public class SecurityOptions
{
    public const string NAME = "SecurityOptions";
    public int MaxLoginAttempts { get; set; } = 5;
}
