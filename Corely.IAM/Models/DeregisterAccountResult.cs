﻿namespace Corely.IAM.Models;

public record DeregisterAccountResult(
    bool IsSuccess,
    string? Message)
    : ResultBase(IsSuccess, Message);
