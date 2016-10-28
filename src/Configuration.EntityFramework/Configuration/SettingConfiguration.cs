using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Configuration.EntityFramework
{
    public class SettingConfiguration : EntityMappingConfiguration<SettingEntity>
    {
        public override void Map(EntityTypeBuilder<SettingEntity> b)
        {
            b.ToTable("Setting", "Configuration");

            b.HasIndex(e => e.SectionId)
                .HasName("IX_Setting_SectionId");

            b.HasIndex(e => new { e.SectionId, e.Key })
                .HasName("IX_Setting_SectionId_Key")
                .IsUnique();

            b.Property(e => e.Key)
                .IsRequired()
                .HasMaxLength(50);

            b.Property(e => e.ModifiedUser).HasMaxLength(50);

            b.Property(e => e.Timestamp)
                .IsRequired()
                .HasColumnType("timestamp")
                .ValueGeneratedOnAddOrUpdate();

            b.HasOne(d => d.Section)
                .WithMany(p => p.Settings)
                .HasForeignKey(d => d.SectionId);
        }
    }
}