using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Configuration.EntityFramework
{
    public class EFConfigurationSource : IConfigurationSource
    {
        protected virtual Action<DbContextOptionsBuilder> OptionsAction { get; set; }

        protected virtual ConfigurationContext Context { get; set; }

        protected virtual string Application { get; set; }

        protected virtual string Aspect { get; set; }

        protected virtual string Discriminator { get; set; }

        protected virtual bool EnsureCreated { get; set; }

        public EFConfigurationSource(string application, string discriminator = null, string aspect = "settings", bool ensureCreated = false)
        {
            this.Application = application;
            this.Discriminator = discriminator;
            this.Aspect = aspect;
            this.EnsureCreated = ensureCreated;
        }

        public EFConfigurationSource(Action<DbContextOptionsBuilder> options, string application = null, string discriminator = null, string aspect = "settings", bool ensureCreated = false)
        {
            this.OptionsAction = options;
            this.Application = application;
            this.Discriminator = discriminator;
            this.Aspect = aspect;
            this.EnsureCreated = ensureCreated;
        }

        public EFConfigurationSource(ConfigurationContext context, string application = null, string discriminator = null, string aspect = "settings", bool ensureCreated = false)
        {
            this.Context = context;
            this.Application = application;
            this.Discriminator = discriminator;
            this.Aspect = aspect;
            this.EnsureCreated = ensureCreated;
        }

        public EFConfigurationSource(string discriminator = null, bool ensureCreated = false)
        {
            this.Discriminator = discriminator;
            this.EnsureCreated = ensureCreated;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return this.Context != null ? new EFConfigurationProvider(this.Context, this.Application, this.Discriminator, this.Aspect, this.EnsureCreated) : new EFConfigurationProvider(this.OptionsAction, this.Application, this.Discriminator, this.Aspect, this.EnsureCreated);
        }
    }
}