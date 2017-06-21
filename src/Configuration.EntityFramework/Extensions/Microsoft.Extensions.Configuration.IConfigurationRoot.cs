using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Configuration.EntityFramework
{
    public static class IConfigurationRootExtensions
    {
        public static bool SectionExists(this IConfigurationRoot configuration, string section)
        {
            return configuration.GetSection(section) != null;
        }

        public static T TryGetSection<T>(this IConfigurationRoot configuration, string section, bool loadDefaultValues = true) where T : class, new()
        {
            return SectionExists(configuration, section) ? GetSection<T>(configuration, section, loadDefaultValues) : null;
        }

        public static T GetSection<T>(this IConfigurationRoot configuration, string section, bool loadDefaultValues = true) where T : class, new()
        {
            var instance = new T();
            if (loadDefaultValues) configuration.LoadDefaultValues(instance);
            configuration.GetSection(section).Bind(instance);
            return instance;
        }

        public static void LoadDefaultValues(this IConfigurationRoot root, object obj)
        {
            var properties = obj.GetType().GetTypeInfo().GetProperties().Where(p => p.GetCustomAttributes(typeof(DefaultValueAttribute), true).Any());
            foreach (var property in properties.Where(p => p.PropertyType.GetTypeInfo().IsSimple()))
            {
                var attribute = (DefaultValueAttribute)property.GetCustomAttributes(typeof(DefaultValueAttribute), true).First();
                var value = Convert.ChangeType(attribute.Value, property.PropertyType);
                property.SetValue(obj, value);
            }
            foreach (var property in properties.Where(p => !p.PropertyType.GetTypeInfo().IsSimple() && !p.PropertyType.IsArray))
            {
                var child = Activator.CreateInstance(property.PropertyType);
                root.LoadDefaultValues(child);
                property.SetValue(obj, child);
            }
        }
    }
}