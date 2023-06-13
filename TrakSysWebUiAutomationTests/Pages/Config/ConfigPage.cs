using Microsoft.Playwright;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsWebUiAutomationTests.Pages.Config
{
    public class ConfigPage : BasePage
    {
        private IPage _page;
        public ConfigPage(IPage page) : base(page)
        {
            _page = page;
        }
        public IPage Page => _page;


        //hub 1 elements/links
        public ILocator Hub1Links => _page.Locator("div[class='tssidebar tssidebar-layout-on']");
        public ILocator LnkSystems => Hub1Links.GetByText("Systems");
        public ILocator LnkTags => Hub1Links.GetByText("Tags");

        //slice 1 elements
        public ILocator Slice1 => _page.Locator("div#tsslice-index-1");

        //slice 2 elements
        public ILocator Slice2 => _page.Locator("div#tsslice-index-2");
        public ILocator BtnNew => Slice2.GetByRole(AriaRole.Link, new() { Name = "New" });

        //slice 3 elements
        public ILocator Slice3 => _page.Locator("div#tsslice-index-3");

        //new tag config page
        public ILocator TxtTagName => _page.GetByLabel("Name", new() { Exact = true });
        public ILocator SelDataType => _page.GetByRole(AriaRole.Combobox, new() { Name = "Data Type" });
        public ILocator BtnSaveTag => _page.GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true });
        public ILocator TxtScriptClassName => _page.GetByLabel("Script Class Name", new() { Exact = true });
        public ILocator LnkEvaluation => _page.GetByRole(AriaRole.Tab, new() { Name = "Evaluation" });
        public ILocator BtnSourceTagPicker1 => _page.Locator("#contentPage_tseditSource01TagID_Picker");
        public ILocator BtnSourceTagPicker2 => _page.Locator("#contentPage_tseditSource02TagID_Picker");
        public ILocator TxtTagSearch => _page.GetByLabel("Tag Name");
        public ILocator BtnFindTags => _page.GetByRole(AriaRole.Button, new() { Name = "Find Tags" });


        public async Task CreateNewVirtualTagInExistingGroup(string tagName, string dataType, string[]? tagGroups)
        {
            await LnkTags.ClickAsync();

            foreach(var group in tagGroups)
            {
                await Slice1.GetByText(group).ClickAsync();
            }

            await BtnNew.ClickAsync();
            await _page.GetByRole(AriaRole.Link, new() { Name = $"Virtual Tag" }).ClickAsync();
            await TxtTagName.FillAsync(tagName);
            await SelDataType.SelectOptionAsync(new[] { dataType });
            await BtnSaveTag.ClickAsync();
        }

        public async Task CreateNewScriptClassTagInExistingGroup(string tagName, string dataType, string scriptClassName, string[] sourceTags, string[]? tagGroups)
        {
            await LnkTags.ClickAsync();

            foreach (var group in tagGroups)
            {
                await Slice1.GetByText(group).ClickAsync();
            }

            await BtnNew.ClickAsync();
            await _page.GetByRole(AriaRole.Link, new() { Name = "Script Class Tag" }).ClickAsync();
            await TxtTagName.FillAsync(tagName);
            await SelDataType.SelectOptionAsync(new[] { dataType });
            await TxtScriptClassName.FillAsync(scriptClassName);
            await LnkEvaluation.ClickAsync();

            int i = 1;
            foreach(var tag in sourceTags)
            {
                await _page.Locator($"#contentPage_tseditSource0{i}TagID_Picker").ClickAsync();
                await TxtTagSearch.FillAsync(tag);
                await BtnFindTags.ClickAsync();
                await _page.Locator($"li[title='{tag}']").GetByRole(AriaRole.Button, new() { Name = "Assign" }).ClickAsync();
                i++;
            }

            await BtnSaveTag.ClickAsync();
        }
    }
}
