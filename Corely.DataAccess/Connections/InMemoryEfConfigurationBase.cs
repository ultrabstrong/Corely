﻿using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.Connections
{
    public abstract class InMemoryEFConfigurationBase : IEFConfiguration
    {
        private class EFDbTypes : IEFDbTypes
        {
            // types don't appear to matter for in-memory database
            public string UTCDateColumnType => nameof(UTCDateColumnType);
            public string UTCDateColumnDefaultValue => nameof(UTCDateColumnDefaultValue);
        }

        private readonly EFDbTypes _efDbTypes = new();

        public abstract void Configure(DbContextOptionsBuilder optionsBuilder);

        public IEFDbTypes GetDbTypes() => _efDbTypes;
    }
}
