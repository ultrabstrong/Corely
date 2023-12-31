﻿namespace Corely.Domain.Models.Users
{
    public class UserDetails
    {
        public string? Name { get; init; }
        public string? Phone { get; init; }
        public string? Address { get; init; }
        public byte[]? ProfilePicture { get; init; }
    }
}
