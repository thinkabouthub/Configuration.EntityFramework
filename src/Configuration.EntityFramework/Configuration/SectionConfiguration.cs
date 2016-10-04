using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Configuration.EntityFramework
{
    public class SectionConfiguration : EntityMappingConfiguration<SectionEntity>
    {
        public override void Map(EntityTypeBuilder<SectionEntity> b)
        {
            b.HasIndex(i => new { i.ApplicationName, i.SectionName }).HasName("IX_Section_ApplicationName_SectionName").IsUnique();

            b.Property(p => p.ApplicationName).HasMaxLength(50);
            b.Property(p => p.SectionName).HasMaxLength(50);
            b.Property(p => p.ModifiedUser).HasMaxLength(50);

            b.ToTable("Section", "Configuration").HasKey(p => p.Id);

            b.Ignore(e => e.Settings);
            b.Property(p => p.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
            b.HasMany(e => e.Settings).WithOne(e => e.Section).HasForeignKey(e => e.SectionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}