using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Configuration.EntityFramework
{
    public class EFConfigurationSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;

        private readonly ConfigurationContext _context;

        private readonly string _application;

        private readonly bool _ensureCreated;

        public EFConfigurationSource(bool ensureCreated = false)
        {
            this._ensureCreated = ensureCreated;
        }

        public EFConfigurationSource(string application, bool ensureCreated = false)
        {
            this._application = application;
            this._ensureCreated = ensureCreated;
        }

        public EFConfigurationSource(string application, Action<DbContextOptionsBuilder> optionsAction, bool ensureCreated = false)
        {
            this._application = application;
            this._optionsAction = optionsAction;
            this._ensureCreated = ensureCreated;
        }

        public EFConfigurationSource(string application, ConfigurationContext context, bool ensureCreated = false)
        {
            _application = application;
            _context = context;
            this._ensureCreated = ensureCreated;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return _context != null ? new EFConfigurationProvider(this._application, this._context, this._ensureCreated) : new EFConfigurationProvider(this._application, this._optionsAction, this._ensureCreated);
        }
    }
}