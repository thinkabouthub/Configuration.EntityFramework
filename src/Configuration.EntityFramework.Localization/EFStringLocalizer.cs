using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace Configuration.EntityFramework.Localization
{
    public class EfStringLocalizer : IStringLocalizer
{
        private readonly ConfigurationContext Context;
        private readonly string CultureName;
        private const string AspectName = "Localization";

        public EfStringLocalizer(ConfigurationContext context) : this(context, CultureInfo.CurrentUICulture) { }

        public EfStringLocalizer(ConfigurationContext context, CultureInfo cultureInfo)
        {
            this.Context = context;
            this.CultureName = cultureInfo.Name;
        }

        public virtual LocalizedString this[string name]
        {
            get
            {
                var value = this.GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public virtual LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        protected virtual string GetString(string name)
        {
            var section = this.Context.Sections.FirstOrDefault(s => s.Aspect == AspectName && (string.IsNullOrEmpty(this.CultureName) || s.SectionName == this.CultureName));
            if (section != null)
            {
                var setting = this.Context.Settings.FirstOrDefault(s => s.SectionId == section.Id && s.Key == name);
                return setting?.GetValue<string>();
            }
            return name;
        }

        public virtual IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new EfStringLocalizer(this.Context, culture);
        }

        public virtual IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
        {
            var section = this.Context.Sections.FirstOrDefault(s => s.Aspect == AspectName && (string.IsNullOrEmpty(this.CultureName) || s.SectionName == this.CultureName));
            if (section != null)
            {
                return this.Context.Settings.Select(s => new LocalizedString(s.Key, s.GetValue<string>(), true));
            }
            return new Collection<LocalizedString>();
        }
    }
}
