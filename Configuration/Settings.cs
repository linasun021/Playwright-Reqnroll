using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentStoresTests.Configuration
{
    public class Settings
    {
        public string Environment { get; set; }
        public string BrowserType { get; set; }
        public string UIStoreBaseUrl { get; set; }
        public string UIAdminBaseUrl { get; set; }
        public string APIBaseUrl { get; set; }
        public bool Headless { get; set; }
        public bool Screenshot { get; set; }
        public string ScreenshotDir { get; set; }
        public bool EnableTracing { get; set; }
        public string TraceFile { get; set; }
        public bool EnableVideo { get; set; }
        public string VideoDir { get; set; }
    }
}
