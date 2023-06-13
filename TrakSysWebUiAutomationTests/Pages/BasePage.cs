using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using TsWebUiAutomationTests.Pages.Dev;
using TsWebUiAutomationTests.Pages.Config;
using TsWebUiAutomationTests.Pages.Admin;


namespace TsWebUiAutomationTests.Pages
{
    public class BasePage : PageTest
    {
        private readonly IPage _page;

        private ILocator _lnkConfig => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']");
        private ILocator _lnkDev => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Developer']");
        private ILocator _lnkAdmin => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Administration']");
        private ILocator _lnkSearch => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Search']");
        private ILocator _lnkNotifications => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Notifications']");
        private ILocator _btnUsername => _page.Locator("#navbar-tsuser").GetByText("administrator");
        private ILocator _lnkSettings => _btnUsername.GetByText("Settings");
        private ILocator _lnkLogOff => _btnUsername.GetByText("Log Off");



        public BasePage(IPage page) { _page = page; }

        public async Task<DevPage> GoToDevPage()
        {
            await _lnkDev.ClickAsync();
            return new DevPage(_page);
        }

        public async Task<ConfigPage> GoToConfigPage()
        {
            await _lnkConfig.ClickAsync();
            return new ConfigPage(_page);
        }

        public async Task<AdminPage> GoToAdminPage()
        {
            await _lnkAdmin.ClickAsync();
            return new AdminPage(_page);
        }

        public async Task GoToSearchPage() => await _lnkSearch.ClickAsync();


        public async Task Screenshot()
        {
            string timestamp = DateTime.Now.ToString("yy_MM_dd HH_mm_ss fff");
            await _page.ScreenshotAsync(new()
            {
                Path = $"screenshots/{TestContext.CurrentContext.Test.MethodName} {timestamp}.png"
            });
        }
    }
}
