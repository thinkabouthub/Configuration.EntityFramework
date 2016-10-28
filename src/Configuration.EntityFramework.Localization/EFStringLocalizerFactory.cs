using System;
using Microsoft.Extensions.Localization;

namespace Configuration.EntityFramework.Localization
{
    public class EFStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ConfigurationContext Context;

        public EFStringLocalizerFactory(ConfigurationContext context)
        {
            this.Context = context;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new EFStringLocalizer(this.Context);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new EFStringLocalizer(this.Context);
        }
    }
}
