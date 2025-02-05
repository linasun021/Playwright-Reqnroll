using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reqnroll.Infrastructure;
using System.IO;
using EquipmentStoresTests.Drivers;
using Allure.Net.Commons;

namespace EquipmentStoresTests.Support
{
    public static class ScreenShotsHelper
    {
        public static void TakeScreenshot(IPage page)
        {
            var screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), $"{UIDriverFactory.GetSettings().ScreenshotDir}{Path.GetRandomFileName()}.png");
            page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = screenshotPath,
                FullPage = true
            }).Wait();
            AllureApi.AddAttachment(screenshotPath);
        }
    }
}
