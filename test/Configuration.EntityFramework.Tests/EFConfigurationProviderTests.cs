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

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "appSettings", Aspect = "Application", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var setting = new SettingEntity() { SectionId = section.Id, Key = "TestSetting", Json = @"Test Value", ModifiedUser = "TestUser" };
            setting.ValueType = null;

            context.Settings.Add(setting);
            context.SaveChanges();

            this.Fixture.ClearChangeTracker();

            var builder = new ConfigurationBuilder().AddEntityFrameworkConfig(context, "EFConfigurationProviderTests");
            var configuration = builder.Build();

            var value = configuration.GetValue<string>("TestSetting");
            Assert.True(value == "Test Value");
        }

        [Fact]
        public void Given_BasicTypeSettingWithDescriminator_When_DescriminatorIsValid_Then_SettingRetrieved()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "appSettings1", Aspect = "Application", Discriminator = @"{""Username"":""Patrick""}", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var setting = new SettingEntity() { SectionId = section.Id, Key = "TestSetting1", Json = @"Test Value", ModifiedUser = "TestUser" };
            setting.ValueType = null;

            context.Settings.Add(setting);
            context.SaveChanges();

            this.Fixture.ClearChangeTracker();

            var builder = new ConfigurationBuilder().AddEntityFrameworkConfig(context, "EFConfigurationProviderTests", @"{""Username"":""Patrick""}");
            var configuration = builder.Build();

            var value = configuration.GetValue<string>("TestSetting1");
            Assert.True(value == "Test Value");
        }

        [Fact]
        public void Given_BasicTypeSettingWithDescriminator_When_MultiDescriminatorIsValid_Then_SettingRetrieved()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "appSettings2", Aspect = "Application", Discriminator = @"{""Environment"":""Testing"", ""Username"":""Patrick""}", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var setting = new SettingEntity() { SectionId = section.Id, Key = "TestSetting2", Json = @"Test Value", ModifiedUser = "TestUser" };
            setting.ValueType = null;

            context.Settings.Add(setting);
            context.SaveChanges();

            this.Fixture.ClearChangeTracker();

            var builder = new ConfigurationBuilder().AddEntityFrameworkConfig(context, "EFConfigurationProviderTests", @"{""Environment"":""Testing"", ""Username"":""Patrick""}");
            var configuration = builder.Build();

            var value = configuration.GetValue<string>("TestSetting2");
            Assert.True(value == "Test Value");
        }

        [Fact]
        public void Given_BasicTypeSettingWithDescriminator_When_MultiDescriminatorValueInValid_Then_SettingNotRetrieved()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "appSettings3", Aspect = "Application", Discriminator = @"{""Environment"":""Testing"", ""Username"":""Patrick""}", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var setting = new SettingEntity() { SectionId = section.Id, Key = "TestSetting3", Json = @"""Test Value""", ModifiedUser = "TestUser" };
            setting.ValueType = null;

            context.Settings.Add(setting);
            context.SaveChanges();

            this.Fixture.ClearChangeTracker();

            var builder = new ConfigurationBuilder().AddEntityFrameworkConfig(context, "EFConfigurationProviderTests", @"{""Environment"":""Testing"", ""Username"":""Patrick1""}");
            var configuration = builder.Build();

            var value = configuration.GetValue<string>("TestSetting3");
            Assert.Null(value);
        }

        [Fact]
        public void Given_BasicTypeSettingWithDescriminator_When_MultiDescriminatorKeyInValid_Then_SettingNotRetrieved()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "appSettings4", Aspect = "Application", Discriminator = @"{""Environment"":""Testing""}", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var setting = new SettingEntity() { SectionId = section.Id, Key = "TestSetting4", Json = @"""Test Value""", ModifiedUser = "TestUser" };
            setting.ValueType = null;

            context.Settings.Add(setting);
            context.SaveChanges();

            this.Fixture.ClearChangeTracker();

            var builder = new ConfigurationBuilder().AddEntityFrameworkConfig(context, "EFConfigurationProviderTests", @"{""Environment"":""Testing"", ""Username"":""Patrick""}");
            var configuration = builder.Build();

            var value = configuration.GetValue<string>("TestSetting4");
            Assert.Null(value);
        }

        [Fact]
        public void Given_BasicTypeSettingWithDefault_When_SettingLoaded_Then_SettingRetrieved()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var defaultSection = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "appSettings5", Aspect = "Application", ModifiedUser = "TestUser" };
            context.Sections.Add(defaultSection);
            context.SaveChanges();

            var setting1 = new SettingEntity() { SectionId = defaultSection.Id, Key = "TestSetting5", Json = @"""Default Test Value""", ModifiedUser = "TestUser" };
            setting1.ValueType = null;

            context.Settings.Add(setting1);

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "appSettings6", Aspect = "Application", Discriminator = @"{""Username"":""Patrick""}", ModifiedUser = "TestUser" };
            context.Sections.Add(section);
            context.SaveChanges();

            var setting2 = new SettingEntity() { SectionId = section.Id, Key = "TestSetting6", Json = @"Test Value", ModifiedUser = "TestUser" };
            setting2.ValueType = null;

            context.Settings.Add(setting2);
            context.SaveChanges();

            this.Fixture.ClearChangeTracker();

            var builder = new ConfigurationBuilder()
                .AddEntityFrameworkConfig(context, "EFConfigurationProviderTests")
                .AddEntityFrameworkConfig(context, "EFConfigurationProviderTests", @"{""Username"":""Patrick""}");
            var configuration = builder.Build();

            var value = configuration.GetValue<string>("TestSetting6");
            Assert.True(value == "Test Value");
        }

        [Fact]
        public void Given_ComplexTypeSection_When_IsValid_Then_SectionRetrieved()
        {
            var context = this.Fixture.GetContext<ConfigurationContext>();

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "TestSection1", Aspect = "Application", ModifiedUser = "TestUser" };
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

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "TestSection2", Aspect = "Application", ModifiedUser = "TestUser" };
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

            var section = new SectionEntity() { ApplicationName = "EFConfigurationProviderTests", SectionName = "TestSection3", Aspect = "Application", ModifiedUser = "TestUser" };
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
