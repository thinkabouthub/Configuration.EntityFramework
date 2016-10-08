using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Configuration.EntityFramework
{
    public class ConfigurationContext : DbContext
    {
        public ConfigurationContext()
        {
        }

        public ConfigurationContext(DbContextOptions<ConfigurationContext> options)
            : base(options)
        {
        }

        public ConfigurationContext(DbContextOptions<ConfigurationContext> options, ILoggerFactory loggerFactory)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var config = builder.Build();
                optionsBuilder.UseSqlServer(config.GetConnectionString("ConfigurationContext"));
            }
        }

        public DbSet<SectionEntity> Sections { get; set; }

        public DbSet<SettingEntity> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddEntityConfigurationsFromAssembly(GetType().GetTypeInfo().Assembly);
        }
    }
}