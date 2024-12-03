﻿using Corely.IAM.Security.Models;

namespace Corely.IAM.Accounts.Models;

public class Account
{
    public int Id { get; init; }
    public string AccountName { get; init; } = null!;
    public List<SymmetricKey>? SymmetricKeys { get; set; } = null!;
    public List<AsymmetricKey>? AsymmetricKeys { get; set; } = null!;
}
