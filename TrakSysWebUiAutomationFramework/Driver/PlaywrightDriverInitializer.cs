using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrakSysWebUiAutomationFramework.Config;

namespace TrakSysWebUiAutomationFramework.Driver
{
    public class PlaywrightDriverInitializer : IPlaywrightDriverInitializer
    {
        public const float DEFAULT_TIMEOUT = 30f; //default timeout, 30 sec by default



        public async Task<IBrowser> GetChromiumDriverAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "chromium";

            return await GetBrowserAsync(DriverType.Chromium, options);
        }

        public async Task<IBrowser> GetChromeDriverAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "chrome";

            return await GetBrowserAsync(DriverType.Chrome, options);
        }

        public async Task<IBrowser> GetEdgeDriverAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "msedge";

            return await GetBrowserAsync(DriverType.Edge, options);
        }

        public async Task<IBrowser> GetFirefoxDriverAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "firefox";

            return await GetBrowserAsync(DriverType.Firefox, options);
        }

        public async Task<IBrowser> GetWebkitDriverAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "webkit";

            return await GetBrowserAsync(DriverType.WebKit, options);
        }

        private async Task<IBrowser> GetBrowserAsync(DriverType driverType, BrowserTypeLaunchOptions options)
        {
            var playwright = await Playwright.CreateAsync();

            return await playwright[driverType.ToString().ToLower()].LaunchAsync(options);
        }

        private BrowserTypeLaunchOptions GetLaunchOptions(string[]? args, float? timeout = DEFAULT_TIMEOUT, bool? headless = true, float? slowmo = null)
        {
            return new BrowserTypeLaunchOptions()
            {
                Args = args,
                Timeout = ToMilliseconds(timeout),
                Headless = headless,
                SlowMo = slowmo
            };
        }

        private float? ToMilliseconds(float? seconds)
        {
            return seconds * 1000;
        }
    }
}
