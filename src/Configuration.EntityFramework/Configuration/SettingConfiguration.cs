using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Configuration.EntityFramework
{
    public class SettingConfiguration : EntityMappingConfiguration<SettingEntity>
    {
        public override void Map(EntityTypeBuilder<SettingEntity> b)
        {
            b.HasIndex(i => new { i.SectionId, i.Key }).HasName("IX_Setting_SectionId_Key").IsUnique();

            b.Property(p => p.Key).HasMaxLength(50);
            b.Property(p => p.ModifiedUser).HasMaxLength(50);

            b.ToTable("Setting", "Configuration").HasKey(p => p.Id);

            //b.HasKey(e => new { e.Id, e.SectionId });
            b.Property(e => e.Json).HasColumnName("Value");
            b.Property(p => p.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }
}