using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsWebUiAutomationTests.Pages.Dev
{
    public class DevPage : BasePage
    {
        private IPage _page;

        private ILocator _hubLinks => _page.Locator("div[class='tssidebar tssidebar-layout-on']");

        public DevPage(IPage page) : base(page) 
        {
            _page = page;
        }
    }
}
