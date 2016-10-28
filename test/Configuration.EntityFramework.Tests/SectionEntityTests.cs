using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Configuration.EntityFramework.Tests
{
    public class SectionEntityTests : IClassFixture<DbContextFixture<ConfigurationContext>>
    {
        protected virtual DbContextFixture<ConfigurationContext> Fixture { get; private set; }

        public SectionEntityTests(DbContextFixture<ConfigurationContext> fixture)
        {
            fixture.Initialise(this.GetType().Name);
            this.Fixture = fixture;
        }

        [Fact]
        public void Given_AddSection_When_IsValid_Then_SectionPersisted()
        {
            this.Fixture.ClearAll();
            this.Fixture.Context.Sections.Add(new SectionEntity() { ApplicationName = "DbContextSectionTests", SectionName = "appSettings", Aspect = "Settings", ModifiedUser = "TestUser" });
            this.Fixture.Context.SaveChanges();
            Assert.NotNull(this.Fixture.Context.Sections.AsNoTracking().FirstOrDefault(s => s.ApplicationName == "DbContextSectionTests" && s.SectionName == "appSettings" && s.Aspect == "Settings"));
        }

        [Fact]
        public void Given_AddSection_When_ApplicationNameIsNotValid_Then_SectionNotPersisted()
        {
            this.Fixture.ClearAll();
            var exception = Assert.Throws<DbUpdateException>(() =>
            {
                this.Fixture.Context.Sections.Add(new SectionEntity() { ApplicationName = null, SectionName = "appSettings", Aspect = "Settings", ModifiedUser = "TestUser" });
                this.Fixture.Context.SaveChanges();
            });
            Assert.True(exception.InnerException != null && exception.InnerException.Message.StartsWith("Cannot insert the value NULL into column 'ApplicationName'"));
        }

        [Fact]
        public void Given_AddSection_When_SectionNameIsNotValid_Then_SectionNotPersisted()
        {
            this.Fixture.ClearAll();
            var exception = Assert.Throws<DbUpdateException>(() =>
            {
                this.Fixture.Context.Sections.Add(new SectionEntity() { ApplicationName = "DbContextSectionTests", SectionName = null, Aspect = "Settings", ModifiedUser = "TestUser" });
                this.Fixture.Context.SaveChanges();
            });
            Assert.True(exception.InnerException != null && exception.InnerException.Message.StartsWith("Cannot insert the value NULL into column 'SectionName'"));
        }

        [Fact]
        public void Given_AddSection_When_SectionIsDuplicate_Then_SectionNotPersisted()
        {
            this.Fixture.ClearAll();
            var exception = Assert.Throws<DbUpdateException>(() =>
            {
                this.Fixture.Context.Sections.Add(new SectionEntity() { ApplicationName = "DbContextSectionTests.Duplicate", SectionName = "appSettingsC", Aspect = "Settings", ModifiedUser = "TestUser" });
                this.Fixture.Context.Sections.Add(new SectionEntity() { ApplicationName = "DbContextSectionTests.Duplicate", SectionName = "appSettingsC", Aspect = "Settings", ModifiedUser = "TestUser" });
                this.Fixture.Context.SaveChanges();
            });
            Assert.True(exception.InnerException != null && exception.InnerException.Message.StartsWith("Cannot insert duplicate key row in object 'Configuration.Section' with unique index 'IX_Section_ApplicationName_SectionName'"));
        }
    }
}
