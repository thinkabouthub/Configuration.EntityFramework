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

        protected virtual string Descriminator { get; set; }

        protected virtual bool EnsureCreated { get; set; }

        public EFConfigurationSource(string application, string descriminator = null, string aspect = "settings", bool ensureCreated = false)
        {
            this.Application = application;
            this.Descriminator = descriminator;
            this.Aspect = aspect;
            this.EnsureCreated = ensureCreated;
        }

        public EFConfigurationSource(Action<DbContextOptionsBuilder> options, string application = null, string descriminator = null, string aspect = "settings", bool ensureCreated = false)
        {
            this.OptionsAction = options;
            this.Application = application;
            this.Descriminator = descriminator;
            this.Aspect = aspect;
            this.EnsureCreated = ensureCreated;
        }

        public EFConfigurationSource(ConfigurationContext context, string application = null, string descriminator = null, string aspect = "settings", bool ensureCreated = false)
        {
            this.Context = context;
            this.Application = application;
            this.Descriminator = descriminator;
            this.Aspect = aspect;
            this.EnsureCreated = ensureCreated;
        }

        public EFConfigurationSource(string descriminator = null, bool ensureCreated = false)
        {
            this.Descriminator = descriminator;
            this.EnsureCreated = ensureCreated;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return this.Context != null ? new EFConfigurationProvider(this.Context, this.Application, this.Descriminator, this.Aspect, this.EnsureCreated) : new EFConfigurationProvider(this.OptionsAction, this.Application, this.Aspect,  this.Descriminator, this.EnsureCreated);
        }
    }
}