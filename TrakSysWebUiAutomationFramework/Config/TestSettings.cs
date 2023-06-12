using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TsWebUiAutomationFramework.Driver;
using Microsoft.Playwright;

namespace TsWebUiAutomationFramework.Config
{
    public class TestSettings
    {
        public Browser Browser { get; set; }
        public string[]? Args { get; set; }
        public int? Timeout = PlaywrightManager.DefaultTimeout;
        public bool? Headless { get; set; }
        public int? SlowMo { get; set; }
        public bool? Trace { get; set; }
        public bool? Video { get; set; }
        public string? BaseUrl { get; set; }


        public TestSettings() { }

        public static TestSettings GetSettingsFromConfig()
        {
            ConfigReader config = new ConfigReader();

            return new TestSettings()
            {
                Browser = ConvertBrowserNameToBrowser(config.GetSetting("browser")),
                Headless = bool.Parse(config.GetSetting("headless")),
                SlowMo = int.Parse(config.GetSetting("slowmo")),
                Timeout = int.Parse(config.GetSetting("timeout")),
                Trace = bool.Parse(config.GetSetting("trace")),
                Video = bool.Parse(config.GetSetting("video")),
                BaseUrl = config.GetSetting("baseurl"),
            };
        }

        public static TestSettings GetSettingsFromConfig(string configFile)
        {
            ConfigReader config = new ConfigReader(configFile);

            return new TestSettings()
            {
                Browser = ConvertBrowserNameToBrowser(config.GetSetting("browser")),
                Headless = bool.Parse(config.GetSetting("headless")),
                SlowMo = int.Parse(config.GetSetting("slowmo")),
                Timeout = int.Parse(config.GetSetting("timeout")),
                Trace = bool.Parse(config.GetSetting("trace")),
                Video = bool.Parse(config.GetSetting("video")),
                BaseUrl = config.GetSetting("baseurl"),
            };
        }

        public static Browser ConvertBrowserNameToBrowser(string browserName)
        {
            switch (browserName.ToLower())
            {
                case "chromium": return Browser.Chromium;
                case "chrome": return Browser.Chrome;
                case "edge": return Browser.Edge;
                case "firefox": return Browser.Firefox;
                case "webkit": return Browser.WebKit;
                default: return Browser.Chromium;
            }
        }
    }

    public enum Browser
    {
        Chromium,
        Firefox,
        WebKit,
        Edge,
        Chrome
    }
}
