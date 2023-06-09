using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSWebUiAutomationFramework.Driver;

namespace TSWebUiAutomationFramework.Config
{
    public class TestSettings
    {
        public BrowserTypes BrowserType { get; set; }
        public string? BaseUrl { get; set; }
        public string[]? Args { get; set; }
        public int? Timeout = PlaywrightManager.DefaultTimeout;
        public bool? Headless { get; set; }
        public int? SlowMo { get; set; }
    }

    public enum BrowserTypes
    {
        Chromium,
        Firefox,
        WebKit,
        Edge,
        Chrome
    }
}
