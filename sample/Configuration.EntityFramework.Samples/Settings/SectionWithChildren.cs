using System;
using System.Collections.ObjectModel;

namespace Configuration.EntityFramework.Samples
{
    public class SectionWithChildren
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Collection<Child> Children { get; set; }
    }
}
