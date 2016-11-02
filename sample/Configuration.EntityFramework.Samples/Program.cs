using System;
using Microsoft.Extensions.Configuration;

namespace Configuration.EntityFramework.Samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initialise Sample Database and Seed Sample Data
            var db = new Database()
                .Create()
                .Seed();

            MainSample();
            //SettingsForApplicationSample();
            //SettingsWithDiscriminatorSample();
        }

        /// <summary>
        /// Common examples.
        /// </summary>
        public static void MainSample()
        {
            // Initial Configuration System
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", true, true)
                .AddEntityFrameworkConfig().Build();

            // Check Configuration Section Exists
            var exists = config.SectionExists("SectionWithChild");

            // Get Configuration Section of type SectionWithChild. Return null if section does not exist.
            var test1 = config.TryGetSection<SectionWithChild>("SectionWithChild");

            // Get Configuration Section of type SectionWithChild. Return default value if section does not exist.
            var test2 = config.GetSection<SectionWithChild>("SectionWithChild");

            // Get Configuration Section of type SectionWithChildren. Return default value if section does not exist.
            var test3 = config.GetSection<SectionWithChildren>("SectionWithChildren");

            // Get Configuration Value for Key. 
            var test4 = config.GetValue<string>("TestSetting");
        }

        /// <summary>
        /// Retrieve settings for application only.
        /// </summary>
        public static void SettingsForApplicationSample()
        {
            // Initial Configuration System
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", true, true)
                // Only load settings using EntityFramework Configuration Provider for "SampleApplication" application
                .AddEntityFrameworkConfig("SampleApplication").Build();

            var test = config.GetValue<string>("TestSetting");
        }

        /// <summary>
        /// Retrieve settings filtered by discriminator.
        /// </summary>
        public static void SettingsWithDiscriminatorSample()
        {
            // Initial Configuration System
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", true, true)
                // Only load settings for user "Patrick"
                .AddEntityFrameworkConfig("SampleApplication", @"{""Username"":""Patrick""}").Build();

            var test = config.GetValue<string>("UserTestSetting");
        }
    }
}
