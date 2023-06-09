using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrakSysWebUiAutomationFramework.Driver;

namespace TrakSysWebUiAutomationFramework.Config
{
    public class TestSettings
    {
        public DriverType DriverType { get; set; }
        public string BaseUrl { get; set; }
        public string[]? Args { get; set; }
        public float? Timeout = PlaywrightDriverInitializer.DEFAULT_TIMEOUT;
        public bool? Headless { get; set; }
        public float? SlowMo { get; set; }
    }

    public enum DriverType
    {
        Chromium,
        Firefox,
        WebKit,
        Edge,
        Chrome,
    }
}
