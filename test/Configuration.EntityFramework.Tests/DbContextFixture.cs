using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Configuration.EntityFramework.Tests
{
    public enum DatabaseBuildOption
    {
        Create,
        Migration
    }

    public class DbContextFixture : IDisposable
    {
        public virtual IConfigurationRoot Configuration { get; private set; }

        public virtual Guid Identifier { get; private set; }

        private bool disposed;

        public DbContextFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", true, true);
            this.Configuration = builder.Build();

            this.Identifier = Guid.NewGuid();
            this._context = new Collection<DbContext>();
            this.DatabaseBuildOption = DatabaseBuildOption.Create;
        }

        private Collection<DbContext> _context;

        public virtual DatabaseBuildOption DatabaseBuildOption { get; set; }

        public virtual IEnumerable<DbContext> Context => this._context;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void InitialiseContext<T>(string identifier = null) where T : DbContext
        {
            this.InitialiseContextWithName<T>(typeof(T).Name, identifier);
        }

        protected virtual Type GetDeclaringType()
        {
            int index = 1;
            var type = new StackFrame(index, true).GetMethod().DeclaringType;
            while (type != null && (type.Equals(this.GetType()) || type.Equals(this.GetType().BaseType)))
            {
                index++;
                type = new StackFrame(index, true).GetMethod().DeclaringType;
            }
            return type;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void InitialiseContextWithName<T>(string connectionName, string identifier = null) where T : DbContext
        {
            var type = this.GetDeclaringType();
            var name = identifier ?? type?.Name;

            var optionsBuilder = new DbContextOptionsBuilder<T>();
            var connection = this.Configuration.GetConnectionString(connectionName);
            if (string.IsNullOrEmpty(connection)) connection = this.Configuration.GetConnectionString("DefaultConnection");
            var connectionBuilder = new SqlConnectionStringBuilder(connection);
            connectionBuilder.InitialCatalog += string.IsNullOrEmpty(name) ? $"_{this.Identifier}" : $"_{name}";
            optionsBuilder.UseSqlServer(connectionBuilder.ConnectionString);
            this.InitialiseContext(optionsBuilder.Options);
        }

        public virtual void InitialiseContextWithConnection<T>(string connection) where T : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseSqlServer(connection);
            this.InitialiseContext(optionsBuilder.Options);
        }

        public virtual void InitialiseContext<T>(DbContextOptions<T> options) where T : DbContext
        {
            if (!this.Context.Any(c => c is T))
            {
                var context = (T)Activator.CreateInstance(typeof(T), options);
                context.Database.EnsureDeleted();
                if (this.DatabaseBuildOption == DatabaseBuildOption.Migration)
                {
                    context.Database.Migrate();
                }
                else if (this.DatabaseBuildOption == DatabaseBuildOption.Create)
                {
                    context.Database.EnsureCreated();
                }
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                this._context.Add(context);
            }
        }

        public virtual T GetContext<T>(bool clearChangeTracker = true) where T : DbContext
        {
            var context = this.Context.OfType<T>().FirstOrDefault();
            if (context != null && clearChangeTracker)
            {
                this.ClearChangeTracker(context);
            }
            return context;
        }

        public virtual void ClearChangeTracker(DbContext context)
        {
            var entries = context.ChangeTracker.Entries();
            while (entries.Any())
            {
                entries.First().State = EntityState.Detached;
            }
        }

        public virtual void ClearChangeTracker()
        {
            foreach (var context in this.Context)
            {
                this.ClearChangeTracker(context);
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
                while (this.Context.Any())
                {
                    var context = this.Context.First();
                    //context.Database.EnsureDeleted();
                    context.Dispose();
                }

            }
            disposed = true;
        }
    }

}
