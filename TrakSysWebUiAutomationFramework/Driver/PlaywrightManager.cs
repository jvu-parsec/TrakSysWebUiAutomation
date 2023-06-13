using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsWebUiAutomationFramework.Config;

namespace TsWebUiAutomationFramework.Driver
{
    public class PlaywrightManager
    {
        private readonly Lazy<Task<IBrowser>> _browser;
        private readonly Lazy<Task<IBrowserContext>> _context;
        private readonly Lazy<Task<IPage>> _page;
        private readonly TestSettings _testSettings;
        public const int DefaultTimeout = 30000; //default timeout in milliseconds, 30 sec by default

        private ConfigReader config = new ConfigReader();

        public PlaywrightManager(TestSettings settings)
        {
            _testSettings = settings;

            _browser = new Lazy<Task<IBrowser>>(SetupPlaywrightAndBrowser);
            _context = new Lazy<Task<IBrowserContext>>(GetNewContextAsync);
            if ((bool) settings.Trace)
            {
                StartTracing(_context.Value.Result);
            }

            _page = new Lazy<Task<IPage>>(GetNewPageAsync);
            //_browser = Task.Run(SetupPlaywrightAndBrowser);
            //_context = Task.Run(GetNewContextAsync);
            //if ((bool)settings.Trace) 
            //{ 
            //    StartTracing(_context.Result); 
            //}

            //_page = Task.Run(GetNewPageAsync);
        }

        public Task<IPage> Page => _page.Value;  //expression syntax; same as this: public IPage Page() { return _page.Result; }
        public Task<IBrowser> Browser => _browser.Value;  //expression syntax; same as this: public IPage Page() { return _page.Result; }
        public Task<IBrowserContext> Context => _context.Value;  //expression syntax; same as this: public IPage Page() { return _page.Result; }

        public static async Task StartTracing(IBrowserContext context)
        {
            await context.Tracing.StartAsync(new()
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        public static async Task StopTracing(IBrowserContext context)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss fff");

            await context.Tracing.StopAsync(new()
            {
                Path = $"traceviewer/{TestContext.CurrentContext.Test.MethodName} {timestamp} TRACE.zip",
            });
        }

        private async Task<IBrowser> SetupPlaywrightAndBrowser()
        {
            switch(_testSettings.Browser)
            {
                case Config.Browser.Chromium:
                    return await GetChromiumBrowserAsync(_testSettings);
                case Config.Browser.Chrome: 
                    return await GetChromeBrowserAsync(_testSettings);
                case Config.Browser.Edge: 
                    return await GetEdgeBrowserAsync(_testSettings);
                case Config.Browser.Firefox: 
                    return await GetFirefoxBrowserAsync(_testSettings);
                case Config.Browser.WebKit: 
                    return await GetWebkitBrowserAsync(_testSettings);
                default: 
                    return await GetChromiumBrowserAsync(_testSettings);
            };
        }

        private async Task<IPage> GetNewPageAsync()
        {
            //return await _browser.Result.NewPageAsync();
            return await _context.Value.Result.NewPageAsync();
        }
        
        private async Task<IBrowserContext> GetNewContextAsync()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss fff");

            var options = new BrowserNewContextOptions()
            {
                RecordVideoDir = $"videos/{TestContext.CurrentContext.Test.MethodName} {timestamp}",
                RecordVideoSize = new RecordVideoSize() { Width = 1280, Height = 1024 }
            };

            return bool.Parse(config.GetSetting("video")) ? await (await _browser.Value).NewContextAsync(options) : await (await _browser.Value).NewContextAsync();
        }

        private async Task<IBrowser> GetChromiumBrowserAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "chromium";

            return await GetBrowserAsync(Config.Browser.Chromium, options);
        }

        private async Task<IBrowser> GetChromeBrowserAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "chrome";

            return await GetBrowserAsync(Config.Browser.Chromium, options);
        }

        private async Task<IBrowser> GetEdgeBrowserAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "msedge";

            return await GetBrowserAsync(Config.Browser.Chromium, options);
        }

        private async Task<IBrowser> GetFirefoxBrowserAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "firefox";

            return await GetBrowserAsync(Config.Browser.Firefox, options);
        }

        private async Task<IBrowser> GetWebkitBrowserAsync(TestSettings settings)
        {
            var options = GetLaunchOptions(settings.Args, settings.Timeout, settings.Headless, settings.SlowMo);
            options.Channel = "webkit";

            return await GetBrowserAsync(Config.Browser.WebKit, options);
        }

        private async Task<IBrowser> GetBrowserAsync(Browser browserType, BrowserTypeLaunchOptions options)
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
