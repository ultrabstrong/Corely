using Corely.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.DataSources.EntityFramework.Configurations
{
    internal class GenericEntityTypeConfiguration
    {
        public static void Configure<T>(EntityTypeBuilder<T> builder) where T : class
        {
            if (typeof(T).Name.EndsWith("Entity"))
            {
                string tableName = typeof(T).Name.Replace("Entity", "");
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
                    .HasDefaultValueSql(SqlConstants.GETUTCDATE)
                    .IsRequired();
            }

            if (typeof(IHasModifiedUtc).IsAssignableFrom(typeof(T)))
            {
                builder.Property(e => ((IHasModifiedUtc)e).ModifiedUtc)
                    .HasDefaultValueSql(SqlConstants.GETUTCDATE)
                    .IsRequired();
            }
        }
    }
}
