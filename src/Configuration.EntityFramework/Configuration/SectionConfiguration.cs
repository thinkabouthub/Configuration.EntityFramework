using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Configuration.EntityFramework
{
    public class SectionConfiguration : EntityMappingConfiguration<SectionEntity>
    {
        public override void Map(EntityTypeBuilder<SectionEntity> b)
        {
            b.ToTable("Section", "Configuration");

            b.HasIndex(e => new { e.ApplicationName, e.Aspect })
                .HasName("IX_Section_ApplicationName_Aspect");

            b.HasIndex(e => new { e.ApplicationName, e.SectionName })
                .HasName("IX_Section_ApplicationName_SectionName")
                .IsUnique();

            b.Property(e => e.ApplicationName)
                .IsRequired()
                .HasMaxLength(50);

            b.Property(e => e.Aspect).HasMaxLength(50);

            b.Property(e => e.Modified).HasDefaultValueSql("getdate()");

            b.Property(e => e.ModifiedUser).HasMaxLength(50);

            b.Property(e => e.SectionName)
                .IsRequired()
                .HasMaxLength(50);

            b.Property(e => e.Timestamp)
                .IsRequired()
                .HasColumnType("timestamp")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}