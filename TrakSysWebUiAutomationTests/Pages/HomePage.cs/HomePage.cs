using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsWebUiAutomationTests.Pages.HomePage.cs
{
    public class HomePage : BasePage
    {
        private readonly IPage _page;

        private ILocator _lnkTsLogo => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']");
        private ILocator _lnkSite => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']");
        private ILocator _lnkConfig => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']");
        private ILocator _lnkDev => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']");
        private ILocator _lnkAdmin => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']");
        private ILocator _lnkSearch => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']");
        private ILocator _lnkNotifications => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']");
        private ILocator _btnUser => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']");
        private ILocator _lnkSettings => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']");
        private ILocator _lnkLogOff => _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']");


        public HomePage(IPage page) : base(page)
        {
            _page = page;
        }
    }
}
