using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipmentStoresTests.Configuration;

namespace EquipmentStoresTests.Drivers
{
    public class APIDriverFactory
    {
        private static Settings _settings;

        static APIDriverFactory()
        {
            var configReader = new ConfigurationReader();
            _settings = configReader.GetSettings();
        }

        public static async Task<IAPIRequestContext> CreateAPIConext(string baseUrl)
        {
            var playwright = await Playwright.CreateAsync();

            return await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
            {
                BaseURL = baseUrl,
                IgnoreHTTPSErrors = true
            });
        }
        public static Settings GetSettings()
        {
            return _settings;
        }
    }
}
