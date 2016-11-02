using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Configuration.EntityFramework
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, string application = null, string discriminator = null, string aspect = "settings", bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(application, discriminator, aspect, ensureCreated));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, Action<DbContextOptionsBuilder> options, string application = null, string discriminator = null, string aspect = "settings", bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(options, application, discriminator, aspect, ensureCreated));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, ConfigurationContext context, string application = null, string discriminator = null, string aspect = "settings", bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(context, application, discriminator, aspect, ensureCreated));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, string discriminator, bool ensureCreated)
        {
            return builder.Add(new EFConfigurationSource(discriminator, ensureCreated));
        }
    }
}