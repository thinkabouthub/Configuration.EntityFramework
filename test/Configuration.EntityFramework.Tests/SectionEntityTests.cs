using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Configuration.EntityFramework.Tests
{
    public class SectionEntityTests : IClassFixture<DbContextFixture>
    {
        protected virtual DbContextFixture Fixture { get; private set; }

        public SectionEntityTests(DbContextFixture fixture)
        {
            fixture.InitialiseContext<ConfigurationContext>();
            this.Fixture = fixture;
        }

        [Fact]
        public void Given_AddSection_When_IsValid_Then_SectionPersisted()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            context.Sections.Add(new SectionEntity() { ApplicationName = "DbContextSectionTests", SectionName = "appSettings", Aspect = "settings", Descriminator = @"{""Environment"":""Testing"", ""Username"":""Patrick""}", ModifiedUser = "TestUser" });
            context.SaveChanges();
            Assert.NotNull(context.Sections.AsNoTracking().FirstOrDefault(s => s.ApplicationName == "DbContextSectionTests" && s.SectionName == "appSettings" && s.Aspect == "Settings"));
        }

        [Fact]
        public void Given_AddSection_When_ApplicationNameIsNotValid_Then_SectionNotPersisted()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var exception = Assert.Throws<DbUpdateException>(() =>
            {
                context.Sections.Add(new SectionEntity() { ApplicationName = null, SectionName = "appSettings", Aspect = "settings", ModifiedUser = "TestUser" });
                context.SaveChanges();
            });
            Assert.True(exception.InnerException != null && exception.InnerException.Message.StartsWith("Cannot insert the value NULL into column 'ApplicationName'"));
        }

        [Fact]
        public void Given_AddSection_When_SectionNameIsNotValid_Then_SectionNotPersisted()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var exception = Assert.Throws<DbUpdateException>(() =>
            {
                context.Sections.Add(new SectionEntity() { ApplicationName = "DbContextSectionTests", SectionName = null, Aspect = "settings", ModifiedUser = "TestUser" });
                context.SaveChanges();
            });
            Assert.True(exception.InnerException != null && exception.InnerException.Message.StartsWith("Cannot insert the value NULL into column 'SectionName'"));
        }

        //[Fact]
        //public void Given_AddSection_When_SectionNameIsDuplicate_Then_SectionNotPersisted()
        //{
        //    var context = this.Fixture.GetContext<ConfigurationContext>();

        //    var exception = Assert.Throws<DbUpdateException>(() =>
        //    {
        //        context.Sections.Add(new SectionEntity() { ApplicationName = "DbContextSectionTests.Duplicate", SectionName = "appSettingsC", Aspect = "settings", ModifiedUser = "TestUser" });
        //        context.Sections.Add(new SectionEntity() { ApplicationName = "DbContextSectionTests.Duplicate", SectionName = "appSettingsC", Aspect = "settings", ModifiedUser = "TestUser" });
        //        context.SaveChanges();
        //    });
        //    Assert.True(exception.InnerException != null && exception.InnerException.Message.StartsWith("Cannot insert duplicate key row in object 'Configuration.Section' with unique index 'IX_Section_ApplicationName_Aspect_SectionName'"));
        //}
    }
}
