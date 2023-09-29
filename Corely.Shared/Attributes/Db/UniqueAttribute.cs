﻿namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UniqueAttribute : Attribute
    {
        public bool? Validate { get; init; }
    }
}
