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

        public EFConfigurationSource(string application, Action<DbContextOptionsBuilder> optionsAction)
        {
            _application = application;
            _optionsAction = optionsAction;
        }

        public EFConfigurationSource(string application, ConfigurationContext context)
        {
            _application = application;
            _context = context;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return _context != null ? new EFConfigurationProvider(_application, _context) : new EFConfigurationProvider(_application, _optionsAction);
        }
    }
}