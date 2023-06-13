using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Configuration;
using TsWebUiAutomationFramework.Driver;
using TsWebUiAutomationFramework.Config;
using TsWebUiAutomationTests.Pages.Login;
using System.Text.RegularExpressions;
using TsWebUiAutomationTests.Pages.Dev;
using TsWebUiAutomationTests.Pages.Admin;
using TsWebUiAutomationFramework.Utils;
using TsWebUiAutomationTests.Pages.Config;

namespace TsWebUiAutomationTests.E2E_Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class TagCreationTests : PageTest
    {
        private TestSettings _testSettings = TestSettings.GetSettingsFromConfig();
        private IPage _page;

        [Test, Retry(1)]
        public async Task Test1_CreateAndCompileTagScriptClass()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                //create new Tag Script Class
                DevPage devPage = new DevPage(_page);
                await devPage.GoToDevPage();
                await devPage.CreateNewLogicScriptInExistingGroup("Tag Script Class", "TEST1_FillerOverheatedScript", new string[] { "TESTGROUP" });
                await devPage.EditScript("TEST1_FillerOverheatedScript", "Test1_CreateAndCompileTagScriptClass.txt", new string[] { "TESTGROUP" });
                await devPage.Screenshot();

                //ASSERTION: new Tag Script should compile successfully
                await devPage.TestScript();
                await Expect(devPage.Page.GetByText("Code successfully compiled.")).ToBeVisibleAsync();
                await devPage.Screenshot();
                await devPage.CloseEditorMessage();

                //clean up env after test
                //await page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Developer']").ClickAsync();    //delete script class
                //await page.GetByRole(AriaRole.Link, new() { Name = "Scripts" }).ClickAsync();
                //await page.GetByRole(AriaRole.Link, new() { Name = "TESTGROUP" }).ClickAsync();
                //await page.GetByRole(AriaRole.Link, new() { Name = "TEST1_FillerOverheatedScript" }).ClickAsync();
                //await page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                //await page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
            });
        }

        //[Test, Retry(1)]
        //[Ignore("Ignore test")]
        //public async Task Test2_CreateScriptTagAndLinkScriptAndSourceTags()
        //{
        //    Assert.DoesNotThrowAsync(async () =>
        //    {
        //        TsUtils.StopLogicService();

        //        //create new Virtual (Source) Tags and a new Script Class Tag
        //        ConfigPage configPage = new ConfigPage(_page);
        //        await configPage.GoToConfigPage();
        //        await configPage.CreateNewVirtualTagInExistingGroup("TEST2.Filler.Running.Tag", "Discrete", new string[] { "TESTGROUP", "TESTAREA", "TESTLINE 1" });
        //        await configPage.CreateNewVirtualTagInExistingGroup("TEST2.Filler.Temperature.Tag", "Integer", new string[] { "TESTGROUP", "TESTAREA", "TESTLINE 1" });
        //        await configPage.CreateNewScriptClassTagInExistingGroup("TEST2.Filler.Overheated.Tag", "String", "TEST1_FillerOverheatedScript", new string[] { "TEST2.Filler.Temperature.Tag", "TEST2.Filler.Running.Tag" }, new string[] { "TESTGROUP", "TESTAREA", "TESTLINE 1" });

        //        //ASSERTION: new tags should be visible in slice 2
        //        await Expect(configPage.Slice2.GetByText("TEST2.Filler.Running.Tag")).ToBeVisibleAsync();
        //        await Expect(configPage.Slice2.GetByText("TEST2.Filler.Temperature.Tag")).ToBeVisibleAsync();
        //        await Expect(configPage.Slice2.GetByText("TEST2.Filler.Overheated.Tag")).ToBeVisibleAsync();
        //        await configPage.Screenshot();

        //        //restart Logic Service and navigate to tags page in Logic Admin
        //        TsUtils.RestartLogicService();

        //        //ASSERTION: Logic Service should be running
        //        AdminPage adminPage = await configPage.GoToAdminPage();
        //        await adminPage.TileStatus.GetByText("Running").WaitForAsync();
        //        await Expect(adminPage.Status).ToHaveTextAsync("Running");
        //        await Expect(adminPage.TileStatus).ToHaveClassAsync(new Regex(".*tscolor-green-light.*"));
        //        await Expect(adminPage.TileStatus.GetByText("Scan MS")).ToBeVisibleAsync();
        //        await adminPage.Screenshot();

        //        //filter to find newly created tags
        //        await adminPage.ClickTagsLink();
        //        await adminPage.FilterTags(new string[] { "TEST2.FILLER" });

        //        //ASSERTION: new tags should be recognized by Logic Service and visible in Logic Service Tags page
        //        await Expect(adminPage.TblTags.GetByText("TEST2.Filler.Running.Tag")).ToBeVisibleAsync();
        //        await Expect(adminPage.TblTags.GetByText("TEST2.Filler.Temperature.Tag")).ToBeVisibleAsync();
        //        await Expect(adminPage.TblTags.GetByText("TEST2.Filler.Overheated.Tag")).ToBeVisibleAsync();
        //        await adminPage.Screenshot();
        //    });
        //}

        //[Test, Retry(1)]
        //[Ignore("Ignore test")]
        //[TestCase("1", "80", "Not Overheated")]
        //[TestCase("1", "100", "Overheated")]
        //[TestCase("0", "100", "Not Overheated")]
        //public async Task Test3_EditSourceTagValuesAndVerifyScriptTagValue(string runningTagValue, string temperatureTagValue, string expectedValue)
        //{
        //    Assert.DoesNotThrowAsync(async () =>
        //    {
        //        //navigate to tags view, filter for test tags, and update tag values
        //        AdminPage adminPage = new AdminPage(_page);
        //        await adminPage.GoToAdminPage();
        //        await adminPage.ClickTagsLink();
        //        await adminPage.FilterTags(new string[] { "TEST2.Filler" });
        //        await adminPage.Screenshot();

        //        //ASSERTION: Script Tag value should update according to changes in Source Tag values
        //        await adminPage.EditTagValue("TEST2.Filler.Running.Tag", runningTagValue);
        //        await adminPage.EditTagValue("TEST2.Filler.Temperature.Tag", temperatureTagValue);
        //        await adminPage.ClickUpdatingTab();
        //        await Expect(adminPage.TrTagItem("TEST2.Filler.Overheated.Tag").GetByText(expectedValue, new() { Exact = true })).ToBeVisibleAsync();
        //    });
        //}

        //[Test, Retry(1)]
        //[Ignore("Ignore test")]
        //public async Task Test4_ConfigureScriptClassTag_Overheating()
        //{
        //    Assert.DoesNotThrowAsync(async () =>
        //    {
        //        TsUtils.StopLogicService();

        //        //create new Tag Script Class
        //        DevPage devPage = new DevPage(_page);
        //        await devPage.GoToDevPage();
        //        await devPage.CreateNewLogicScriptInExistingGroup("Tag Script Class", "TEST4_FillerOverheatedScript", new string[] { "TESTGROUP" });
        //        await devPage.EditScript("TEST4_FillerOverheatedScript", "Test4_ConfigureScriptClassTag_Overheating.txt", new string[] { "TESTGROUP" });
        //        await devPage.Screenshot();

        //        //ASSERTION: new tag script should compile successfully
        //        await devPage.TestScript();
        //        await Expect(devPage.Page.GetByText("Code successfully compiled.")).ToBeVisibleAsync();
        //        await devPage.Screenshot();
        //        await devPage.CloseEditorMessage();

        //        //create new Virtual (Source) Tags and a new Script Class Tag
        //        ConfigPage configPage = await devPage.GoToConfigPage();
        //        await configPage.CreateNewVirtualTagInExistingGroup("TEST4.Filler.Running.Tag", "Discrete", new string[] { "TESTGROUP", "TESTAREA", "TESTLINE 1" });
        //        await configPage.CreateNewVirtualTagInExistingGroup("TEST4.Filler.Temperature.Tag", "Integer", new string[] { "TESTGROUP", "TESTAREA", "TESTLINE 1" });
        //        await configPage.CreateNewScriptClassTagInExistingGroup("TEST4.Filler.Overheated.Tag", "String", "TEST4_FillerOverheatedScript", new string[] { "TEST4.Filler.Temperature.Tag", "TEST4.Filler.Running.Tag" }, new string[] { "TESTGROUP", "TESTAREA", "TESTLINE 1" });

        //        //ASSERTION: new tags should be visible in slice 2
        //        await Expect(configPage.Slice2.GetByText("TEST4.Filler.Running.Tag")).ToBeVisibleAsync();
        //        await Expect(configPage.Slice2.GetByText("TEST4.Filler.Temperature.Tag")).ToBeVisibleAsync();
        //        await Expect(configPage.Slice2.GetByText("TEST4.Filler.Overheated.Tag")).ToBeVisibleAsync();
        //        await configPage.Screenshot();

        //        //restart Logic Service
        //        TsUtils.RestartLogicService();

        //        //ASSERTION: Logic Service should be running
        //        AdminPage adminPage = await configPage.GoToAdminPage();
        //        await adminPage.TileStatus.GetByText("Running").WaitForAsync();
        //        await Expect(adminPage.Status).ToHaveTextAsync("Running");
        //        await Expect(adminPage.TileStatus).ToHaveClassAsync(new Regex(".*tscolor-green-light.*"));
        //        await Expect(adminPage.TileStatus.GetByText("Scan MS")).ToBeVisibleAsync();
        //        await adminPage.Screenshot();

        //        //navigate to tags view, filter for test tags, and update tag values
        //        await adminPage.ClickTagsLink();
        //        await adminPage.FilterTags(new string[] { "TEST4.Filler" });
        //        await adminPage.Screenshot();

        //        //ASSERTIONS:
        //        //Test Case 1: running = 1, temperature = 80, expected = "Not Overheated"
        //        await adminPage.EditTagValue("TEST4.Filler.Running.Tag", "1");
        //        await adminPage.EditTagValue("TEST4.Filler.Temperature.Tag", "80");
        //        await adminPage.ClickUpdatingTab();
        //        await Expect(adminPage.TrTagItem("TEST4.Filler.Overheated.Tag").GetByText("Not Overheated", new() { Exact = true })).ToBeVisibleAsync();

        //        //Test Case 2: running = 1, temperature = 100, expected = "Overheated"
        //        await adminPage.FilterTags(new string[] { "TEST4.Filler" });
        //        await adminPage.EditTagValue("TEST4.Filler.Temperature.Tag", "100");
        //        await adminPage.ClickUpdatingTab();
        //        await Expect(adminPage.TrTagItem("TEST4.Filler.Overheated.Tag").GetByText("Overheated", new() { Exact = true })).ToBeVisibleAsync();

        //        //Test Case 3: running = 0, temperature = 100, expected = "Not Overheated"
        //        await adminPage.FilterTags(new string[] { "TEST4.Filler" });
        //        await adminPage.EditTagValue("TEST4.Filler.Running.Tag", "0");
        //        await adminPage.ClickUpdatingTab();
        //        await Expect(adminPage.TrTagItem("TEST4.Filler.Overheated.Tag").GetByText("Not Overheated", new() { Exact = true })).ToBeVisibleAsync();

        //        //clean up test env after test
        //        //TsUtils.StopLogicService();
        //        //await _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']").ClickAsync();    //delete tags
        //        //await _page.GetByRole(AriaRole.Link, new() { Name = "Tags", Exact = true }).ClickAsync();
        //        //await _page.GetByText("TESTGROUP").ClickAsync();
        //        //await _page.GetByText("TESTAREA").ClickAsync();
        //        //await _page.GetByText("TESTLINE 1").ClickAsync();
        //        //await _page.Locator("#tsslice-index-2").GetByText("TEST.Filler.Overheated.Tag").ClickAsync();
        //        //await _page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        //        //await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
        //        //await _page.Locator("#tsslice-index-2").GetByText("TEST.Filler.Running.Tag").ClickAsync();
        //        //await _page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        //        //await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
        //        //await _page.Locator("#tsslice-index-2").GetByText("TEST.Filler.Temperature.Tag").ClickAsync();
        //        //await _page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        //        //await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
        //        //await _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Developer']").ClickAsync();    //delete script class
        //        //await _page.GetByRole(AriaRole.Link, new() { Name = "Scripts" }).ClickAsync();
        //        //await _page.GetByRole(AriaRole.Link, new() { Name = "TESTGROUP" }).ClickAsync();
        //        //await _page.GetByRole(AriaRole.Link, new() { Name = "TEST_FillerOverheatedScript" }).ClickAsync();
        //        //await _page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        //        //await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
        //    });
        //}

        [SetUp]
        public async Task Setup()
        {
            //playwright->browser->context->page: create new playwright instance, browser instance, set browser context via config file values, get new page instance
            var playwright = new PlaywrightManager(_testSettings);
            _page = await playwright.Page;

            LoginPage loginPage = new LoginPage(_page);
            await loginPage.Login("administrator", "Test123$$");
        }

        [TearDown]
        public async Task Teardown()
        {
            if ((bool) _testSettings.Trace)
            {
                await PlaywrightManager.StopTracing(_page.Context);
            }

            await _page.Context.CloseAsync();
            await _page.Context.Browser.CloseAsync();
        }

        [OneTimeTearDown]
        public async Task CleanUpTestEnv()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                LoginPage loginPage = new LoginPage(_page);
                await loginPage.Login("administrator", "Test123$$");
                TsUtils.StopLogicService();

                // clean up Tag Script Class for Test 1
                await _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Developer']").ClickAsync();    //delete script class
                await _page.GetByRole(AriaRole.Link, new() { Name = "Scripts" }).ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "TESTGROUP" }).ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "TEST1_FillerOverheatedScript" }).ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();

                // clean up tags for Test 2
                await _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']").ClickAsync();    //delete tags
                await _page.GetByRole(AriaRole.Link, new() { Name = "Tags", Exact = true }).ClickAsync();
                await _page.GetByText("TESTGROUP").ClickAsync();
                await _page.GetByText("TESTAREA").ClickAsync();
                await _page.GetByText("TESTLINE 1").ClickAsync();
                await _page.Locator("#tsslice-index-2").GetByText("TEST2.Filler.Overheated.Tag").ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
                await _page.Locator("#tsslice-index-2").GetByText("TEST2.Filler.Running.Tag").ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
                await _page.Locator("#tsslice-index-2").GetByText("TEST2.Filler.Temperature.Tag").ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();

                // clean up tags for Test 4
                await _page.Locator("#tsslice-index-2").GetByText("TEST4.Filler.Overheated.Tag").ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
                await _page.Locator("#tsslice-index-2").GetByText("TEST4.Filler.Running.Tag").ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
                await _page.Locator("#tsslice-index-2").GetByText("TEST4.Filler.Temperature.Tag").ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
                await _page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Developer']").ClickAsync();    //delete script class
                await _page.GetByRole(AriaRole.Link, new() { Name = "Scripts" }).ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "TESTGROUP" }).ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "TEST4_FillerOverheatedScript" }).ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
            });
        }
    }
}