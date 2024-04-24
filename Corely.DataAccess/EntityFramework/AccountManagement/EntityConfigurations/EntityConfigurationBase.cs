using Corely.DataAccess.EntityFramework.Configurations;
using Corely.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.AccountManagement.EntityConfigurations
{
    internal abstract class EntityConfigurationBase<T>
        : IEntityTypeConfiguration<T>
        where T : class
    {
        protected readonly IEFDbTypes EFDbTypes;

        protected EntityConfigurationBase(IEFDbTypes efDbTypes)
        {
            EFDbTypes = efDbTypes;
        }

        public abstract void Configure(EntityTypeBuilder<T> builder);

        protected void ConfigureGenericTypes(EntityTypeBuilder<T> builder)
        {
            if (typeof(T).Name.EndsWith("Entity"))
            {
                string tableName = typeof(T).Name.Replace("Entity", "");
                if (!tableName.EndsWith('s'))
                {
                    tableName += "s";
                }
                builder.ToTable(tableName);
            }

            if (typeof(IHasIdPk).IsAssignableFrom(typeof(T)))
            {
                builder.HasKey(e => ((IHasIdPk)e).Id);
                builder.Property(e => ((IHasIdPk)e).Id)
                    .ValueGeneratedOnAdd();
            }

            if (typeof(IHasCreatedUtc).IsAssignableFrom(typeof(T)))
            {
                builder.Property(e => ((IHasCreatedUtc)e).CreatedUtc)
                    .HasColumnType(EFDbTypes.UTCDateColumnType)
                    .HasDefaultValueSql(EFDbTypes.UTCDateColumnDefaultValue)
                    .IsRequired();
            }

            if (typeof(IHasModifiedUtc).IsAssignableFrom(typeof(T)))
            {
                builder.Property(e => ((IHasModifiedUtc)e).ModifiedUtc)
                    .HasColumnType(EFDbTypes.UTCDateColumnType)
                    .HasDefaultValueSql(EFDbTypes.UTCDateColumnDefaultValue)
                    .IsRequired();
            }
        }
    }
}
