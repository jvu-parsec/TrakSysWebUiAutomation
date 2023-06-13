using Microsoft.Playwright;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsWebUiAutomationTests.Pages.Admin
{
    public class AdminPage : BasePage
    {
        private IPage _page;
        public IPage Page => _page;
        public AdminPage(IPage page) : base(page) 
        {
            _page = page;
        }

        //hub 1 elements/links
        public ILocator Hub1Links => _page.Locator("div[class='tssidebar tssidebar-layout-on']");

        public ILocator TileStatus => _page.Locator("#TileStatus");
        public ILocator Status => TileStatus.Locator(".tstile-text-small");
        public ILocator DivRelatedLinks => _page.Locator("#MenuRelated");
        public ILocator LnkTags => DivRelatedLinks.GetByText("Tags").Nth(0);
        public ILocator LnkLogs => DivRelatedLinks.GetByText("Logs");
        public ILocator LnkBadTags => DivRelatedLinks.GetByText("Bad Tags");

        //logic service tags page
        public ILocator TxtTagFilter1 => _page.GetByLabel($"Tag Filter 1");
        public ILocator BtnRefresh => _page.GetByRole(AriaRole.Button, new() { Name = "Refresh" });
        public ILocator LnkUpdating => _page.Locator("#MenuTagViewMode").GetByText("Updating");
        public ILocator LnkFilter => _page.Locator("#MenuTagViewMode").GetByText("Filter");
        public ILocator BtnEditTagValue(string tagName) => _page.Locator($"//*[contains(text(),'{tagName}')]/ancestor::tr").GetByRole(AriaRole.Link, new() { Name = "Edit Value" });
        public ILocator BtnSaveTagValue => _page.GetByRole(AriaRole.Link, new() { Name = "Save" });
        public ILocator TrTagItem(string tagName) => _page.Locator($"//*[contains(text(),'{tagName}')]/ancestor::tr");
        public ILocator TblTags => _page.Locator("#TableTags");


        public async Task FilterTags(string[] tagFilters)
        {
            await ClickFilterTab();
            int i = 1;
            foreach(var filter in tagFilters)
            {
                await _page.GetByLabel($"Tag Filter {i}").FillAsync(filter);
                i++;
            }
            await BtnRefresh.ClickAsync();
        }

        public async Task ClickTagsLink() => await LnkTags.ClickAsync();
        public async Task ClickFilterTab() => await LnkFilter.ClickAsync();
        public async Task ClickUpdatingTab() => await LnkUpdating.ClickAsync();

        public async Task EditTagValue(string tagName, string tagValue)
        {
            await BtnEditTagValue(tagName).ClickAsync();
            await _page.GetByLabel("Value").FillAsync(tagValue);
            await BtnSaveTagValue.ClickAsync();
        }
    }
}
