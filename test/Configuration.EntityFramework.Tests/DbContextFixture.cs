using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Configuration.EntityFramework.Tests
{
    public class DbContextFixture<T> : IDisposable where T : DbContext
    {
        public virtual IConfigurationRoot Configuration { get; private set; }

        public virtual T Context { get; private set; }

        public virtual Guid Identifier { get; private set; }

        private bool disposed;

        public DbContextFixture()
        {
            this.Identifier = Guid.NewGuid();
        }

        public virtual void Initialise(string identifier = null)
        {
            this.InitialiseWithName("DefaultConnection", identifier);
        }

        public virtual void InitialiseWithName(string connectionName, string identifier = null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", true, true);
            this.Configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<T>();
            var connectionBuilder = new SqlConnectionStringBuilder(this.Configuration.GetConnectionString(connectionName));
            connectionBuilder.InitialCatalog += string.IsNullOrEmpty(identifier) ? $"_{this.Identifier}" : $"_{identifier}";
            optionsBuilder.UseSqlServer(connectionBuilder.ConnectionString);
            this.Initialise(optionsBuilder.Options);
        }

        public virtual void InitialiseWithConnection(string connection)
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseSqlServer(connection);
            this.Initialise(optionsBuilder.Options);
        }

        public virtual void Initialise(DbContextOptions<T> options)
        {
            this.Context = (T)Activator.CreateInstance(typeof(T), options);
            this.Context.Database.EnsureDeleted();
            this.Context.Database.EnsureCreated();
            this.Context.ChangeTracker.AutoDetectChangesEnabled = false;
            this.SeedData();
        }

        public virtual void SeedData()
        {
            
        }

        public virtual void ClearAll()
        {
            var entries = this.Context.ChangeTracker.Entries();
            while (entries.Any())
            {
                entries.First().State = EntityState.Detached;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                if (this.Context != null)
                {
                    this.Context.Database.EnsureDeleted();
                    this.Context.Dispose();
                    this.Context = null;
                }
            }
            disposed = true;
        }
    }

}
