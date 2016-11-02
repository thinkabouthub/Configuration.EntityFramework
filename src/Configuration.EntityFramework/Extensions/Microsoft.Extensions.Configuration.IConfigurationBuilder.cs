using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Configuration.EntityFramework
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, string application, string descriminator = null, string aspect = "settings", bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(application, descriminator, aspect, ensureCreated));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder,  Action<DbContextOptionsBuilder> options, string application = null, string descriminator = null, string aspect = "settings", bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(options, application, descriminator, aspect, ensureCreated));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, ConfigurationContext context, string application = null, string descriminator = null, string aspect = "settings", bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(context, application, descriminator, aspect, ensureCreated));
        }

        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, string descriminator = null, bool ensureCreated = false)
        {
            return builder.Add(new EFConfigurationSource(descriminator, ensureCreated));
        }
    }
}