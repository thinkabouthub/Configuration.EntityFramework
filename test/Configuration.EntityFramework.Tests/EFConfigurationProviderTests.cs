using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Configuration.EntityFramework.Tests
{
    public class EFConfigurationProviderTests : IClassFixture<DbContextFixture>
    {
        protected virtual DbContextFixture Fixture { get; private set; }

        public EFConfigurationProviderTests(DbContextFixture fixture)
        {
            fixture.InitialiseContext<ConfigurationContext>();
            this.Fixture = fixture;
        }

        [Fact]
        public void Given_BasicTypeSetting_When_IsValid_Then_SettingRetrieved()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "appSettings", Aspect = "Settings", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var setting = new SettingEntity() { SectionId = section.Id, Key = "TestSetting", Json = @"""Test Value""", ModifiedUser = "TestUser" };
            setting.ValueType = null;

            context.Settings.Add(setting);
            context.SaveChanges();

            this.Fixture.ClearChangeTracker();

            var builder = new ConfigurationBuilder().AddEntityFrameworkConfig(context);
            var configuration = builder.Build();

            var value = configuration.GetValue<string>("TestSetting");
            Assert.True(value == "Test Value");
        }

        [Fact]
        public void Given_ComplexTypeSection_When_IsValid_Then_SectionRetrieved()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "TestSection1", Aspect = "Settings", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var setting = new SettingEntity() { SectionId = section.Id, Key = "default", ModifiedUser = "TestUser" };
            setting.SetValue(new TestSectionWithChild() {Id = Guid.NewGuid(), Name = "Test"});
            var value = setting.GetValue<TestSectionWithChild>();

            context.Settings.Add(setting);
            context.SaveChanges();

            this.Fixture.ClearChangeTracker();

            var builder = new ConfigurationBuilder().AddEntityFrameworkConfig(context);
            var configuration = builder.Build();

            var configSection = configuration.GetSection<TestSectionWithChild>("TestSection1", false);
            Assert.NotNull(configSection);
            Assert.Equal(value.Id, configSection.Id);
            Assert.Equal(value.Name, configSection.Name);
        }

        [Fact]
        public void Given_ComplexTypeSectionWithChild_When_IsValid_Then_SectionRetrieved()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "TestSection2", Aspect = "Settings", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var setting = new SettingEntity()
            {
                SectionId = section.Id,
                Key = "default",
                ModifiedUser = "TestUser"
            };
            setting.SetValue(new TestSectionWithChild()
            {
                Id = Guid.NewGuid(),
                Name = "Test1",
                ChildSection = new TestSectionChild()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test2"
                }
            });
            var value = setting.GetValue<TestSectionWithChild>().ChildSection;

            context.Settings.Add(setting);
            context.SaveChanges();

            this.Fixture.ClearChangeTracker();

            var builder = new ConfigurationBuilder().AddEntityFrameworkConfig(context);
            var configuration = builder.Build();

            var configSection = configuration.GetSection<TestSectionWithChild>("TestSection2", false);
            Assert.NotNull(configSection);
            Assert.NotNull(configSection.ChildSection);
            Assert.Equal(value.Id, configSection.ChildSection.Id);
            Assert.Equal(value.Name, configSection.ChildSection.Name);
        }

        [Fact]
        public void Given_ComplexTypeSectionWithChildren_When_IsValid_Then_SectionRetrieved()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "TestSection3", Aspect = "Settings", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var value = new TestSectionWithChildren()
            {
                Id = Guid.NewGuid(),
                Name = "Test1"
            };
            value.Children = new Collection<TestSectionChild>();
            value.Children.Add
            (
                new TestSectionChild()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test1"
                }
            );

            var setting = new SettingEntity()
            {
                SectionId = section.Id,
                Key = "default",
                ModifiedUser = "TestUser"
            };
            setting.SetValue(value);
            context.Settings.Add(setting);
            context.SaveChanges();

            this.Fixture.ClearChangeTracker();

            var builder = new ConfigurationBuilder().AddEntityFrameworkConfig(context);
            var configuration = builder.Build();

            var configSection = configuration.GetSection<TestSectionWithChildren>("TestSection3", false);
            Assert.NotNull(configSection);
            Assert.NotNull(configSection.Children);
            var child = configSection.Children.FirstOrDefault();
            Assert.NotNull(child);
            Assert.Equal(value.Children.First().Id, child.Id);
            Assert.Equal(value.Children.First().Name, child.Name);
        }

        [Fact]
        public void Given_ComplexTypeSectionWithDefaultValue_When_IsValid_Then_SectionInitialisedWithDefaultValue()
        {
            var builder = new ConfigurationBuilder();
            var configuration = builder.Build();

            var configSection = configuration.GetSection<TestSectionWithChild>("TestSection4");
            Assert.NotNull(configSection);
            Assert.True(!string.IsNullOrEmpty(configSection.TestDefault));
            Assert.Equal("Default Value", configSection.TestDefault);
        }

        public class TestSectionWithChild
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            [DefaultValue("Default Value")]
            public string TestDefault { get; set; }

            public TestSectionChild ChildSection { get; set; }
        }

        public class TestSectionWithChildren
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public Collection<TestSectionChild> Children { get; set; }
        }


        public class TestSectionChild
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}
