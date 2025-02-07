using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipmentStoresTests.Configuration;
using System.Windows.Forms;
using System.IO;
using Allure.Net.Commons;
using static System.Net.Mime.MediaTypeNames;


namespace EquipmentStoresTests.Drivers
{
    public class UIDriverFactory
    {
        private static IPlaywright _playwright;
        private static IBrowser _browser;
        private static Settings _settings;
        private static IPage _page;
        private static string _tracingFile;

        static UIDriverFactory()
        {
            var configReader = new ConfigurationReader();
            _settings = configReader.GetSettings();
        }

        public static async Task<IPage> GetPageAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = _settings.BrowserType switch
            {
                "Chromium" => await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = _settings?.Headless }),
                "Firefox" => await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = _settings?.Headless }),
                "WebKit" => await _playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = _settings?.Headless }),
                _ => await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = _settings?.Headless })
            };
            var viewportSize = new ViewportSize
            {
                Width = 1792,
                Height = 1344
            };
            var context = _settings.EnableVideo ?
                await _browser.NewContextAsync(
                                                new BrowserNewContextOptions
                                                {
                                                    ViewportSize = viewportSize,
                                                    RecordVideoDir = _settings.VideoDir,
                                                }) :
                await _browser.NewContextAsync(
                                                 new BrowserNewContextOptions
                                                 {
                                                     ViewportSize = viewportSize,
                                                 });

            _page = await context.NewPageAsync();

            if (_settings.EnableTracing)
            {
                await _page.Context.Tracing.StartAsync(new TracingStartOptions
                {
                    Snapshots = true,
                    Screenshots = true,
                    Sources = true
                });
            }
            return _page;
        }

        public static async Task CloseAsync(string scenarioName)
        {

            if (_settings.EnableTracing)
            {
                _tracingFile = _settings.TraceFile + "_" + scenarioName.Replace(" ", "") + ".zip";
                await _page.Context.Tracing.StopAsync(new TracingStopOptions
                {
                    Path = _tracingFile
                });
            }

            if (_page != null)
            {
                await _page.CloseAsync();
                _page = null;
            }

            if (_browser != null)
            {
                await _browser.CloseAsync();
                _browser = null;
            }
            try
            {
                string videoDir = Path.Combine(Directory.GetCurrentDirectory(), _settings.VideoDir);
                string[] videoFiles = Directory.GetFiles(videoDir, "*.webm");
                foreach (var videoFile in videoFiles)
                {
                    if (!videoFile.EndsWith("_Attached.webm"))
                    {
                        AllureApi.AddAttachment(videoFile);
                        File.Move(videoFile, videoFile.Split(".webm")[0] + "_Attached.webm");
                    }
                }
                string traceFile = Path.Combine(Directory.GetCurrentDirectory(), _tracingFile);
                AllureApi.AddAttachment(traceFile);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            if (_playwright != null)
            {
                _playwright.Dispose();
                _playwright = null;
            }
        }

        public static Settings GetSettings()
        {
            return _settings;
        }

        private class ScreenDimensions
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
