using System;
using Configuration.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.Configuration
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
    }
}