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
        public DevPage(IPage page) : base(page) => _page = page;
        public IPage Page => _page;

        //hub 1 elements
        private ILocator _hubLinks => _page.Locator("div[class='tssidebar tssidebar-layout-on']");
        private ILocator _lnkPageDefinitions => _hubLinks.GetByText("Page Definitions");
        private ILocator _lnkContent => _hubLinks.GetByText("Content");
        private ILocator _lnkPages => _lnkContent.GetByText("Pages");
        private ILocator _lnkParts => _lnkContent.GetByText("Parts");
        private ILocator _lnkLibrary => _hubLinks.GetByText("Library");
        private ILocator _lnkScripts => _hubLinks.GetByText("Scripts");
        private ILocator _lnkLogicScripts => _lnkScripts.GetByText("Logic");
        private ILocator _lnkWebScripts => _lnkScripts.GetByText("Web");
        private ILocator _lnkModuleScripts => _lnkScripts.GetByText("Module");
        private ILocator _lnkGlobalScripts => _lnkScripts.GetByText("Global");
        private ILocator _lnkDbTransferScripts => _lnkScripts.GetByText("Database Transfer");
        private ILocator _lnkWorkflowScripts => _lnkScripts.GetByText("Workflow");
        private ILocator _lnkModules => _hubLinks.GetByText("Modules");
        private ILocator _lnkWorkflow => _hubLinks.GetByText("Workflow");
        private ILocator _lnkStateMachines => _lnkWorkflow.GetByText("State Machines");
        private ILocator _lnkOperations => _hubLinks.GetByText("Operations");
        private ILocator _lnkConnect => _hubLinks.GetByText("Connect");
        private ILocator _lnkPayload => _lnkConnect.GetByText("Payload Definitions");
        private ILocator _lnkInterface => _lnkConnect.GetByText("Interface");

        //slice 1 locators
        private ILocator _divSlice1 => _page.Locator("div#tsslice-index-1");

        //slice 2 locators
        private ILocator _divSlice2 => _page.Locator("div#tsslice-index-2");
        private ILocator _btnNew => _divSlice2.GetByRole(AriaRole.Link, new () { Name = "New" });
        private ILocator _btnTagScriptClass => _divSlice2.GetByRole(AriaRole.Link, new() { Name = "Tag Script Class" });

        //slice 3 elements
        private ILocator _divSlice3 => _page.Locator("#tsslice-index-3");
        private ILocator _lnkScriptAction => _divSlice3.GetByRole(AriaRole.Link, new() { Name = "Script" });

        //script tag creation page
        private ILocator _txtScriptName => _page.GetByPlaceholder("Required");
        private ILocator _btnSave => _page.GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true });
        private ILocator _btnSaveAndNew => _page.GetByRole(AriaRole.Button, new() { Name = "Save and New", Exact = true });
        private ILocator _btnApply => _page.GetByRole(AriaRole.Button, new() { Name = "Apply", Exact = true });
        private ILocator _btnCancel => _page.GetByRole(AriaRole.Button, new() { Name = "Cancel", Exact = true });
        private ILocator _selScriptGroup => _page.GetByRole(AriaRole.Combobox, new() { Name = "Script Group" });

        //script editor elements
        private ILocator _txtEditor => _page.Locator("#contentPage_Editor_Code");
        private ILocator _btnEditorSave => _page.GetByText("Save", new() { Exact = true });
        private ILocator _btnTest => _page.GetByText("Test", new() { Exact = true });
        private ILocator _btnEditorClose => _page.GetByRole(AriaRole.Button, new() { Name = "Close" });
        private ILocator _msgCompileSuccess => _page.GetByText("Code successfully compiled.");
        private ILocator _msgCompileFail => _page.GetByText("Failed Test Compile");
        private ILocator _btnEditorSettings => _page.GetByRole(AriaRole.Link, new () { Name = "Editor configuration" });
        private ILocator _selEditorType => _page.GetByRole(AriaRole.Combobox, new() { Name = "Type" });
        private ILocator _lnkSave => _page.GetByRole(AriaRole.Link, new() { Name = "Save" });


        

        public async Task CreateNewLogicScriptInExistingGroup(string scriptType, string scriptName, string[]? groups)
        {
            await _lnkScripts.ClickAsync();
            foreach(var group in groups)
            {
                await _divSlice1.GetByText(group).ClickAsync();
            }

            await _btnNew.ClickAsync();
            await _btnTagScriptClass.ClickAsync();
            await _txtScriptName.FillAsync(scriptName);
            //await _selScriptGroup.SelectOptionAsync(scriptGroup);
            await _btnSave.ClickAsync();
        }

        public async Task EditScript(string scriptName, string textFile, string[]? groups)
        {
            string textFilePath = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\TestData\\{textFile}";
            string testScript = File.ReadAllText(textFilePath);

            foreach (var group in groups)
            {
                await _divSlice1.GetByText(group).ClickAsync();
            }

            await _divSlice2.GetByText(scriptName).ClickAsync();
            await _lnkScriptAction.ClickAsync();

            await ToggleEditor();

            await _txtEditor.ClearAsync();
            await _txtEditor.FillAsync(testScript);
            await _btnEditorSave.ClickAsync();
        }

        public async Task<bool> TestScript()
        {
            await _btnTest.ClickAsync();

            return await _msgCompileSuccess.IsVisibleAsync();
        }

        public async Task CloseEditorMessage()
        {
            await _btnEditorClose.ClickAsync();
        }

        public async Task ToggleEditor()
        {
            await _btnEditorSettings.ClickAsync();
            await _selEditorType.SelectOptionAsync(new[] { "Text" });
            await _lnkSave.ClickAsync();
            await _page.ReloadAsync();
        }
    }
}
