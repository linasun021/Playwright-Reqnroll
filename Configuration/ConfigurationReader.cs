using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentStoresTests.Configuration
{
    public class ConfigurationReader
    {
        private readonly IConfigurationRoot _configuration;

        public ConfigurationReader(string configFileName = "appsettings.json")
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFileName, optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public string GetSetting(string key)
        {
            return _configuration[key];
        }

        public T GetSection<T>(string sectionName)
        {
            return _configuration.GetSection(sectionName).Get<T>();
        }

        public Settings GetSettings()
        {
            var settings = new Settings();
            _configuration.Bind(settings);
            return settings;
        }
    }
}
