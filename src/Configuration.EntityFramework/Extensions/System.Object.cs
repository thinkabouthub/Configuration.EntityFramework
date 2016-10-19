using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Configuration.EntityFramework
{
    public static class ObjectExtensions
    {
        public static void LoadDefaultValues(this object obj)
        {
            var properties = obj.GetType().GetTypeInfo().GetProperties().Where(p => p.GetCustomAttributes(typeof(DefaultValueAttribute), true).Any());
            foreach (var property in properties)
            {
                var attribute = (DefaultValueAttribute)property.GetCustomAttributes(typeof(DefaultValueAttribute), true).First();
                var value = Convert.ChangeType(attribute.Value, property.PropertyType);
                property.SetValue(obj, value);
            }
        }
    }
}