using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace TsWebUiAutomationTests.Pages
{
    public class BasePage
    {
        private readonly IPage _page;

        private ILocator _lnkConfig => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']");
        private ILocator _lnkDev => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Developer']");
        private ILocator _lnkAdmin => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Administration']");
        private ILocator _lnkSearch => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Search']");
        private ILocator _lnkNotifications => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Notifications']");
        private ILocator _btnUsername => _page.Locator("#navbar-tsuser").GetByText("administrator");


        public BasePage(IPage page) { _page = page; }

        public async Task Screenshot(IPage page)
        {
            string timestamp = DateTime.Now.ToString("yyyy_MM_dd HH_mm_ss fff");
            await page.ScreenshotAsync(new()
            {
                Path = $"screenshots/{TestContext.CurrentContext.Test.MethodName} {timestamp}.png"
            });
        }

        public void RestartLogicService()
        {
            string restartLogicScriptPath = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\PowershellScripts\\RestartLogicService.ps1";

            using (var ps = PowerShell.Create())
            {
                ps.AddScript($"start-process powershell -windowstyle hidden -verb runas -argumentlist '-file {restartLogicScriptPath}'");
                ps.Invoke();
            }
        }

        public void StopLogicService()
        {
            string stopLogicServiceScriptPath = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\PowershellScripts\\StopLogicService.ps1";

            using (var ps = PowerShell.Create())
            {
                ps.AddScript($"start-process powershell -windowstyle hidden -verb runas -argumentlist '-file {stopLogicServiceScriptPath}'");
                ps.Invoke();
            }
        }
    }
}
