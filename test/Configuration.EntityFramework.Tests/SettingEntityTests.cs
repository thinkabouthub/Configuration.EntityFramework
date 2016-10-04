using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Configuration.EntityFramework.Tests
{
    public class SettingEntityTests : IClassFixture<DbContextFixture<ConfigurationContext>>
    {
        protected virtual DbContextFixture<ConfigurationContext> Fixture { get; private set; }

        public SettingEntityTests(DbContextFixture<ConfigurationContext> fixture)
        {
            fixture.Initialise(this.GetType().Name);
            this.Fixture = fixture;
        }

        [Fact]
        public void Given_AddSection_When_IsValid_Then_SectionPersisted()
        {
            this.Fixture.ClearAll();
            var section = new SectionEntity() { ApplicationName = "DbContextSettingTests", SectionName = "appSettings", ModifiedUser = "TestUser" };
            this.Fixture.Context.Sections.Add(section);
            this.Fixture.Context.SaveChanges();

            this.Fixture.Context.Settings.Add(new SettingEntity() { SectionId = section.Id, Key = "Setting", Json = "hello world", DefaultValue = "DefaultValue", ModifiedUser = "TestUser" });
            this.Fixture.Context.SaveChanges();

            Assert.NotNull(this.Fixture.Context.Settings.AsNoTracking().FirstOrDefault(s => s.SectionId == section.Id && s.Key == "Setting" && s.Json == "hello world" && s.DefaultValue == "DefaultValue"));
        }

        [Fact]
        public void Given_AddSection_When_KeyIsNotValid_Then_SectionNotPersisted()
        {
            this.Fixture.ClearAll();
            var section = new SectionEntity() { ApplicationName = "DbContextSettingTests", SectionName = "appSettingsA", ModifiedUser = "TestUser" };
            this.Fixture.Context.Sections.Add(section);
            this.Fixture.Context.SaveChanges();

            var exception = Assert.Throws<DbUpdateException>(() =>
            {
                this.Fixture.Context.Settings.Add(new SettingEntity() { SectionId = section.Id, Key = null, ModifiedUser = "TestUser" });
                this.Fixture.Context.SaveChanges();
            });
            Assert.True(exception.InnerException != null && exception.InnerException.Message.StartsWith("Cannot insert the value NULL into column 'Key'"));
        }

        [Fact]
        public void Given_AddSetting_When_SettingIsDuplicate_Then_SettingNotPersisted()
        {
            this.Fixture.ClearAll();
            var section = new SectionEntity() { ApplicationName = "DbContextSettingTests", SectionName = "appSettingsB", ModifiedUser = "TestUser" };
            this.Fixture.Context.Sections.Add(section);
            this.Fixture.Context.SaveChanges();

            var exception = Assert.Throws<DbUpdateException>(() =>
            {
                this.Fixture.Context.Settings.Add(new SettingEntity() { SectionId = section.Id, Key = "SettingA", ModifiedUser = "TestUser" });
                this.Fixture.Context.Settings.Add(new SettingEntity() { SectionId = section.Id, Key = "SettingA", ModifiedUser = "TestUser" });
                this.Fixture.Context.SaveChanges();
            });
            Assert.True(exception.InnerException != null && exception.InnerException.Message.StartsWith("Cannot insert duplicate key row in object 'Configuration.Setting' with unique index 'IX_Setting_SectionId_Key'"));
        }

        [Fact]
        public void Given_AddSetting_When_ValueIsComplexType_Then_SettingIsPersisted()
        {
            this.Fixture.ClearAll();
            var section = new SectionEntity() { ApplicationName = "DbContextSettingTests", SectionName = "TestSection1", ModifiedUser = "TestUser" };
            this.Fixture.Context.Sections.Add(section);
            this.Fixture.Context.SaveChanges();

            var setting = new SettingEntity() { SectionId = section.Id, Key = "default", ModifiedUser = "TestUser" };
            setting.SetValue(new TestSection() {Id = Guid.NewGuid(), Name = "Test"});
            this.Fixture.Context.Settings.Add(setting);
            this.Fixture.Context.SaveChanges();

            var retrieved = this.Fixture.Context.Settings.FirstOrDefault(s => s.SectionId == section.Id && s.Key == "default");
            Assert.NotNull(retrieved);
            Assert.NotNull(retrieved.GetValue<TestSection>());
        }

        [Fact]
        public void Given_AddSetting_When_ValueIsComplexTypeWithNoValueType_Then_SettingIsPersistedAndValueAsJObjectReturned()
        {
            this.Fixture.ClearAll();
            var section = new SectionEntity() { ApplicationName = "DbContextSettingTests", SectionName = "TestSection2", ModifiedUser = "TestUser" };
            this.Fixture.Context.Sections.Add(section);
            this.Fixture.Context.SaveChanges();

            var setting = new SettingEntity() { SectionId = section.Id, Key = "default", ModifiedUser = "TestUser" };
            setting.SetValue(new TestSection() {Id = Guid.NewGuid(), Name = "Test"});
            setting.ValueType = null;
            this.Fixture.Context.Settings.Add(setting);
            this.Fixture.Context.SaveChanges();

            var retrieved = this.Fixture.Context.Settings.AsNoTracking().FirstOrDefault(s => s.SectionId == section.Id && s.Key == "default");
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
