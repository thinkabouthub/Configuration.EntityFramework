using System;
using Newtonsoft.Json;

namespace Microsoft.Extensions.Configuration
{
    public static class IConfigurationSectionExtensions
    {
        public static T As<T>(this IConfigurationSection section)
        {
            return (T)JsonConvert.DeserializeObject(section.Value, typeof(T));
        }
    }
}
