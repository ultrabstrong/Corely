﻿using Corely.DataAccess.Connections;
using Corely.Domain.Constants.Auth;
using Corely.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.DataSources.EntityFramework.Configurations.Auth
{
    internal class BasicAuthEntityConfiguration : EntityConfigurationBase<BasicAuthEntity>
    {
        public BasicAuthEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<BasicAuthEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(BasicAuthConstants.USERNAME_MAX_LENGTH);

            builder.HasIndex(x => x.Username)
                .IsUnique();

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(BasicAuthConstants.PASSWORD_MAX_LENGTH);
        }
    }
}
