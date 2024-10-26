﻿using Corely.DataAccess.Interfaces.Entities;

namespace Corely.UnitTests.Fixtures
{
    public class EntityFixture : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        public int Id { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
        public virtual NavigationPropertyFixture? NavigationProperty { get; set; }
    }

    public class NavigationPropertyFixture : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        public int Id { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
