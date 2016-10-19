using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Configuration.EntityFramework
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, string application, bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(application, ensureCreated));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, string application, Action<DbContextOptionsBuilder> setup, bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(application, setup, ensureCreated));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder,  Action<DbContextOptionsBuilder> setup, bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(null, setup, ensureCreated));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, string application, ConfigurationContext context, bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(application, context, ensureCreated));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, ConfigurationContext context, bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(null, context, ensureCreated));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(ensureCreated));
        }
    }
}