using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Configuration.EntityFramework
{
    public class SectionEntity
    {
        [Required]
        [Key]
        public virtual int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string ApplicationName { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string SectionName { get; set; }

        public virtual string Discriminator { get; set; }

        [MaxLength(50)]
        public virtual string Aspect { get; set; }

        public virtual DateTime? Modified { get; set; }

        [MaxLength(50)]
        public virtual string ModifiedUser { get; set; }

        [Required]
        public virtual byte[] Timestamp { get; set; }

        public virtual Collection<SettingEntity> Settings { get; set; }
    }
}