using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSWebUiAutomationFramework.Config;

namespace TSWebUiAutomationFramework.Driver
{
    public class PlaywrightManager
    {
        private readonly Task<IBrowser> _browser;
        private readonly Task<IBrowserContext> _context;
        private readonly Task<IPage> _page;
        private readonly TestSettings _testSettings;
        public const int DefaultTimeout = 30000; //default timeout in milliseconds, 30 sec by default

        public PlaywrightManager(TestSettings settings)
        {
            _testSettings = settings;

            _browser = Task.Run(SetupPlaywrightAndBrowser);
            _page = Task.Run(CreatePageAsync);
        }

        public IPage Page => _page.Result;  //expression syntax; same as this: public IPage Page() { return _page.Result; }

        private async Task<IBrowser> SetupPlaywrightAndBrowser()
        {
            switch(_testSettings.BrowserType)
            {
                case BrowserTypes.Chromium:
                    return await GetChromiumBrowserAsync(_testSettings);
                case BrowserTypes.Chrome: 
                    return await GetChromeBrowserAsync(_testSettings);
                case BrowserTypes.Edge: 
                    return await GetEdgeBrowserAsync(_testSettings);
                case BrowserTypes.Firefox: 
                    return await GetFirefoxBrowserAsync(_testSettings);
                case BrowserTypes.WebKit: 
                    return await GetWebkitBrowserAsync(_testSettings);
                default: 
                    return await GetChromiumBrowserAsync(_testSettings);
            };
        }

        private async Task<IPage> GetNewPageAsync()
        {
            return await (await _browser).NewPageAsync();
        }
        
        private async Task<IBrowserContext> GetNewContextAsync(BrowserNewContextOptions contextOptions)
        {
            var options = new BrowserNewContextOptions()
            {
                
            };

            return await (await _browser).NewContextAsync();
        }

        private async Task<IBrowser> GetChromiumBrowserAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "chromium";

            return await GetBrowserAsync(BrowserTypes.Chromium, options);
        }

        private async Task<IBrowser> GetChromeBrowserAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "chrome";

            return await GetBrowserAsync(BrowserTypes.Chromium, options);
        }

        private async Task<IBrowser> GetEdgeBrowserAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "msedge";

            return await GetBrowserAsync(BrowserTypes.Chromium, options);
        }

        private async Task<IBrowser> GetFirefoxBrowserAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "firefox";

            return await GetBrowserAsync(BrowserTypes.Firefox, options);
        }

        private async Task<IBrowser> GetWebkitBrowserAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "webkit";

            return await GetBrowserAsync(BrowserTypes.WebKit, options);
        }

        private async Task<IBrowser> GetBrowserAsync(BrowserTypes browserType, BrowserTypeLaunchOptions options)
        {
            var playwright = await Playwright.CreateAsync();

            return await playwright[browserType.ToString().ToLower()].LaunchAsync(options);
        }

        private BrowserTypeLaunchOptions GetLaunchOptions(string[]? args, float? timeout = DefaultTimeout, bool? headless = true, float? slowmo = null)
        {
            return new BrowserTypeLaunchOptions()
            {
                Args = args,
                Timeout = timeout,
                Headless = headless,
                SlowMo = slowmo
            };
        }
    }
}
