using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Configuration.EntityFramework.Tests
{
    public class SettingEntityTests : IClassFixture<DbContextFixture>
    {
        protected virtual DbContextFixture Fixture { get; private set; }

        public SettingEntityTests(DbContextFixture fixture)
        {
            fixture.InitialiseContext<ConfigurationContext>();
            this.Fixture = fixture;
        }

        [Fact]
        public void Given_AddSection_When_IsValid_Then_SectionPersisted()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "DbContextSettingTests", SectionName = "appSettings", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            context.Settings.Add(new SettingEntity() { SectionId = section.Id, Key = "Setting", Json = "hello world", DefaultValue = "DefaultValue", ModifiedUser = "TestUser" });
            context.SaveChanges();

            Assert.NotNull(context.Settings.AsNoTracking().FirstOrDefault(s => s.SectionId == section.Id && s.Key == "Setting" && s.Json == "hello world" && s.DefaultValue == "DefaultValue"));
        }

        [Fact]
        public void Given_AddSection_When_KeyIsNotValid_Then_SectionNotPersisted()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "DbContextSettingTests", SectionName = "appSettingsA", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var exception = Assert.Throws<DbUpdateException>(() =>
            {
                context.Settings.Add(new SettingEntity() { SectionId = section.Id, Key = null, ModifiedUser = "TestUser" });
                context.SaveChanges();
            });
            Assert.True(exception.InnerException != null && exception.InnerException.Message.StartsWith("Cannot insert the value NULL into column 'Key'"));
        }

        [Fact]
        public void Given_AddSetting_When_SettingIsDuplicate_Then_SettingNotPersisted()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "DbContextSettingTests", SectionName = "appSettingsB", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var exception = Assert.Throws<DbUpdateException>(() =>
            {
                context.Settings.Add(new SettingEntity() { SectionId = section.Id, Key = "SettingA", ModifiedUser = "TestUser" });
                context.Settings.Add(new SettingEntity() { SectionId = section.Id, Key = "SettingA", ModifiedUser = "TestUser" });
                context.SaveChanges();
            });
            Assert.True(exception.InnerException != null && exception.InnerException.Message.StartsWith("Cannot insert duplicate key row in object 'Configuration.Setting' with unique index 'IX_Setting_SectionId_Key'"));
        }

        [Fact]
        public void Given_AddSetting_When_ValueIsComplexType_Then_SettingIsPersisted()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "DbContextSettingTests", SectionName = "TestSection1", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var setting = new SettingEntity() { SectionId = section.Id, Key = "default", ModifiedUser = "TestUser" };
            setting.SetValue(new TestSection() {Id = Guid.NewGuid(), Name = "Test"});
            context.Settings.Add(setting);
            context.SaveChanges();

            var retrieved = context.Settings.FirstOrDefault(s => s.SectionId == section.Id && s.Key == "default");
            Assert.NotNull(retrieved);
            Assert.NotNull(retrieved.GetValue<TestSection>());
        }

        [Fact]
        public void Given_AddSetting_When_ValueIsComplexTypeWithNoValueType_Then_SettingIsPersistedAndValueAsJObjectReturned()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "DbContextSettingTests", SectionName = "TestSection2", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var setting = new SettingEntity() { SectionId = section.Id, Key = "default", ModifiedUser = "TestUser" };
            setting.SetValue(new TestSection() {Id = Guid.NewGuid(), Name = "Test"});
            setting.ValueType = null;
            context.Settings.Add(setting);
            context.SaveChanges();

            var retrieved = context.Settings.AsNoTracking().FirstOrDefault(s => s.SectionId == section.Id && s.Key == "default");
            Assert.NotNull(retrieved);
            Assert.NotNull(retrieved.GetValue<JObject>());
        }

        public class TestSection
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}
