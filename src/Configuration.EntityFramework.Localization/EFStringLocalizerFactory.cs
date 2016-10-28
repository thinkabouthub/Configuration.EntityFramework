using System;
using Microsoft.Extensions.Localization;

namespace Configuration.EntityFramework.Localization
{
    public class EfStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ConfigurationContext Context;

        public EfStringLocalizerFactory(ConfigurationContext context)
        {
            this.Context = context;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new EfStringLocalizer(this.Context);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new EfStringLocalizer(this.Context);
        }
    }
}
