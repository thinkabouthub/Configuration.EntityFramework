using System;
using System.ComponentModel;

namespace Configuration.EntityFramework.Samples
{
    public class SectionWithChild
    {
        public Guid Id { get; set; }

        [DefaultValue("DefaultName")]
        public string Name { get; set; }

        public Child Child { get; set; }
    }
}
