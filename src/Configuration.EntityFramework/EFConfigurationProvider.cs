using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Configuration.EntityFramework
{
    public class EFConfigurationProvider : ConfigurationProvider
    {
        protected string Application;

        public EFConfigurationProvider(string application, Action<DbContextOptionsBuilder> optionsAction)
        {
            this.Application = application;
            this.OptionsAction = optionsAction;
        }

        public EFConfigurationProvider(string application, ConfigurationContext context)
        {
            this.Application = application;
            this.Context = context;
        }

        protected virtual Action<DbContextOptionsBuilder> OptionsAction { get; set; }

        protected virtual ConfigurationContext Context { get; set; }

        protected virtual bool IsContextOwner { get; set; }

        public override void Load()
        {
            this.Data = new Dictionary<string, string>();
            if (this.Context == null)
            {
                var builder = new DbContextOptionsBuilder<ConfigurationContext>();
                this.OptionsAction(builder);
                this.Context = new ConfigurationContext(builder.Options);
                this.IsContextOwner = true;
            }
            try
            {
                foreach (var section in this.Context.Sections.Where(s => string.IsNullOrEmpty(Application) || (s.ApplicationName == Application)).Include(s => s.Settings))
                {
                    foreach (var setting in section.Settings)
                    {
                        var data = JsonConvert.DeserializeObject(setting.Json);
                        var container = data as JContainer;
                        if (container != null)
                        {
                            this.AddJObjectToData(section.SectionName, container);
                        }
                        else
                        {
                            Data.Add(setting.Key, data.ToString());
                        }
                    }
                }
            }
            finally
            {
                if (this.IsContextOwner && this.Context != null)
                {
                    this.Context.Dispose();
                    this.Context = null;
                }
            }
        }

        protected virtual void AddJObjectToData(string section, JContainer json)
        {
            foreach (var kvp in (JObject)json)
            {
                if (kvp.Value is JObject)
                {
                    this.AddJObjectToData($"{section}:{kvp.Key}", (JObject)kvp.Value);
                }
                else if (kvp.Value is JArray)
                {
                    var array = (JArray)kvp.Value;
                    var i = 0;
                    foreach (var item in array.OfType<JObject>())
                    {              
                        this.AddJObjectToData($"{section}:{kvp.Key}:{i}", item);
                        i++;
                    }
                }
                else
                {
                    var value = kvp.Value.Type != JTokenType.Null ? kvp.Value.ToString() : null;
                    this.Data.Add($"{section}:{kvp.Key}", value);
                }
            }
        }
    }
}