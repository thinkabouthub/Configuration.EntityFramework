using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Configuration.EntityFramework
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, string application, Action<DbContextOptionsBuilder> setup)
        {
            return builder.Add(new EFConfigurationSource(application, setup));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder,  Action<DbContextOptionsBuilder> setup)
        {
            return builder.Add(new EFConfigurationSource(null, setup));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, string application, ConfigurationContext context)
        {
            return builder.Add(new EFConfigurationSource(application, context));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, ConfigurationContext context)
        {
            return builder.Add(new EFConfigurationSource(null, context));
        }
    }
}