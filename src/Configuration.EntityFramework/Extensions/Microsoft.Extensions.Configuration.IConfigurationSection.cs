using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Configuration.EntityFramework
{
    public static class IConfigurationSectionExtensions
    {
        public static T As<T>(this IConfigurationSection section)
        {
            return (T)JsonConvert.DeserializeObject(section.Value, typeof(T));
        }
    }
}
