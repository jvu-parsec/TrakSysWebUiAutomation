using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrakSysWebUiAutomationFramework.Config;

namespace TrakSysWebUiAutomationFramework.Driver
{
    public class PlaywrightDriver
    {
        private readonly Task<IPage> _page;
        private readonly TestSettings _testSettings;
        private readonly IPlaywrightDriverInitializer _playwrightDriverInitializer;
        private IBrowser? _browser;

        public PlaywrightDriver(TestSettings settings, IPlaywrightDriverInitializer pwdInitializer)
        {
            _page = Task.Run(InitializePlaywright);
            _testSettings = settings;
            _playwrightDriverInitializer = pwdInitializer;
        }

        public IPage Page => _page.Result;  //expression syntax; same as the code below
        //public IPage Page()
        //{
        //    return _page.Result;
        //}


        public async Task<IPage> InitializePlaywright()
        {
            var playwright = await Playwright.CreateAsync();

            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = _testSettings.Headless,
                SlowMo = _testSettings.SlowMo
            });

            var context = await _browser.NewContextAsync();
            await context.Tracing.StartAsync(new()
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });

            return await context.NewPageAsync();
        }
    }
}
