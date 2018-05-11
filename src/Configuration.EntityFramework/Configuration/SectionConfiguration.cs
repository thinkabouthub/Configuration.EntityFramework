using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Configuration.EntityFramework
{
    public class SectionConfiguration : EntityMappingConfiguration<SectionEntity>
    {
        public override void Map(EntityTypeBuilder<SectionEntity> b)
        {
            b.ToTable("Section", "Configuration").HasKey(e => e.Id);

            b.HasIndex(e => new {e.ApplicationName, e.Aspect, e.SectionName, e.Discriminator})
                .HasName("IX_Section_ApplicationName_Aspect_SectionName_Discriminator").IsUnique();

            b.Property(e => e.Discriminator)
                .IsRequired(false)
                .HasMaxLength(450);

            b.Property(e => e.ApplicationName)
                .IsRequired()
                .HasMaxLength(50);

            b.Property(e => e.Aspect).HasMaxLength(50);

            b.Property(e => e.Tenant);

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