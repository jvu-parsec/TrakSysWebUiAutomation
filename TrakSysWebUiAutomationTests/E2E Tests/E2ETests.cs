using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Configuration;
using TsWebUiAutomationFramework.Driver;
using TsWebUiAutomationFramework.Config;
using TsWebUiAutomationTests.Pages.Login;
using System.Text.RegularExpressions;

namespace TsWebUiAutomationTests.E2E_Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class E2ETests : PageTest
    {
        protected IPage page;
        private TestSettings testSettings = TestSettings.GetSettingsFromConfig();


        [Test, Retry(3)]
        public async Task Test_LoginAdminValidCredentials()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                //login method called in test Setup method, wait for redirect after login and for url to contain some string/fragment
                await page.WaitForURLAsync("**/pages/home/**");

                //after successful login, user should be redirected to home page/dashboard, url should contain "pages/home" fragment, username, TS logo, and admin links should be visible
                await Expect(page).ToHaveURLAsync(new Regex(".*TS/pages/home/.*")); //url to contain fragment "home"
                await Expect(page.Locator("span.navbar-tsuser-text")).ToContainTextAsync("administrator");  //username to be visible
                await Expect(page.Locator("ul[class='navbar-nav d-xl-flex d-none'] li")).ToHaveCountAsync(4);   //4 static navbar links to be visible (config, developer, admin, search)
                await Expect(page.GetByRole(AriaRole.Link, new() { Name = "TrakSYS™" })).ToBeVisibleAsync();    //TS logo to be visible
            });
        }

        [Test, Retry(3)]
        public async Task Test_ConfigureScriptClassTag_Overheating()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                //create new Tag Script Class
                await page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Developer']").ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "Scripts" }).ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "TESTGROUP" }).ClickAsync();
                await page.Locator("#tsslice-index-2").GetByRole(AriaRole.Link, new() { Name = "New" }).ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "Tag Script Class" }).ClickAsync();
                await page.GetByPlaceholder("Required").FillAsync("TEST_FillerOverheatedScript");
                await page.GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true }).ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "TEST_FillerOverheatedScript" }).ClickAsync();
                await Screenshot(page);

                //edit script for new Tag Script Class
                await page.Locator("#tsslice-index-3").GetByRole(AriaRole.Link, new() { Name = "Script" }).ClickAsync();

                //change script editor from HTML5 to Text editor so code from our text file can get sent
                await page.GetByRole(AriaRole.Link, new() { Name = "Editor configuration" }).ClickAsync();
                await page.GetByRole(AriaRole.Combobox, new() { Name = "Type" }).SelectOptionAsync(new[] { "Text" });
                await page.GetByRole(AriaRole.Link, new() { Name = "Save" }).ClickAsync();

                //clear editor text and replace with our test code
                await page.Locator("#contentPage_Editor_Code").ClearAsync();
                string textFilePath = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\TestData\\TD-TagScriptClass_Code.txt";
                string testScript = File.ReadAllText(textFilePath);
                await page.Locator("#contentPage_Editor_Code").FillAsync(testScript);

                //ASSERTION: script should compile successfully
                await page.GetByText("Test", new() { Exact = true }).ClickAsync();
                await Expect(page.GetByText("Code successfully compiled.")).ToBeVisibleAsync();
                await Screenshot(page);

                //save script
                await page.GetByRole(AriaRole.Button, new() { Name = "Close" }).ClickAsync();
                await page.GetByText("Save & Close").ClickAsync();

                //configure Source Tags and Script Class Tag
                await page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']").ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "Tags", Exact = true }).ClickAsync();
                await page.GetByText("TESTGROUP").ClickAsync();
                await page.GetByText("TESTAREA").ClickAsync();
                await page.GetByText("TESTLINE 1").ClickAsync();

                //configure 2 source tags (virtual tags for filler running and filler temperature)
                await page.Locator("#tsslice-index-2").GetByRole(AriaRole.Link, new() { Name = "New" }).ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "Virtual Tag" }).ClickAsync();
                await page.GetByPlaceholder("Required").FillAsync("TEST.Filler.Running.Tag");
                await page.GetByRole(AriaRole.Combobox, new() { Name = "Data Type" }).SelectOptionAsync(new[] { "Discrete" });
                await page.GetByRole(AriaRole.Button, new() { Name = "Save and New" }).ClickAsync();
                await page.GetByPlaceholder("Required").FillAsync("TEST.Filler.Temperature.Tag");
                await page.GetByRole(AriaRole.Combobox, new() { Name = "Data Type" }).SelectOptionAsync(new[] { "Integer" });
                await page.GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true }).ClickAsync();
                await Screenshot(page);

                //configure Script Class Tag
                await page.Locator("#tsslice-index-2").GetByRole(AriaRole.Link, new() { Name = "New" }).ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "Script Class Tag" }).ClickAsync();
                await page.GetByLabel("Name", new() { Exact = true }).FillAsync("TEST.Filler.Overheated.Tag");
                await page.GetByLabel("Script Class Name").FillAsync("TEST_FillerOverheatedScript");
                await page.GetByRole(AriaRole.Tab, new() { Name = "Evaluation" }).ClickAsync();
                await page.Locator("#contentPage_tseditSource01TagID_Picker").ClickAsync();
                await Screenshot(page);

                await page.GetByLabel("Tag Name").FillAsync("TEST.Filler.");
                await page.GetByRole(AriaRole.Button, new() { Name = "Find Tags" }).ClickAsync();
                await page.Locator("li[title='TEST.Filler.Temperature.Tag']").GetByRole(AriaRole.Button, new() { Name = "Assign" }).ClickAsync();
                await page.Locator("#contentPage_tseditSource02TagID_Picker").ClickAsync();
                await page.Locator("li[title='TEST.Filler.Running.Tag']").GetByRole(AriaRole.Button, new() { Name = "Assign" }).ClickAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true }).ClickAsync();

                //ASSERTION: newly configured tags should be visible in slice 2
                await Expect(page.Locator("#tsslice-index-2").GetByText("TEST.Filler.Running.Tag")).ToBeVisibleAsync();
                await Expect(page.Locator("#tsslice-index-2").GetByText("TEST.Filler.Temperature.Tag")).ToBeVisibleAsync();
                await Expect(page.Locator("#tsslice-index-2").GetByText("TEST.Filler.Overheated.Tag")).ToBeVisibleAsync();
                await Screenshot(page);

                //restart Logic service through powershell script (can't start/stop service via TS Web in v12+, also need to disable UAC prompt for running ps w/ elevated rights)
                RestartLogicService();

                await page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Administration']").ClickAsync();

                //ASSERTION: logic service should be running
                //await page.Locator("#TileStatus").GetByText("Starting").WaitForAsync();
                await page.Locator("#TileStatus").GetByText("Running").WaitForAsync();
                await Expect(page.Locator("#TileStatus").GetByText("Running")).ToBeVisibleAsync();
                await Expect(page.Locator("#TileStatus")).ToHaveClassAsync(new Regex(".*tscolor-green-light.*"));
                await Expect(page.Locator("#TileStatus").GetByText("Scan MS")).ToBeVisibleAsync();
                await Screenshot(page);

                //navigate to Tags view, filter to show test tags
                await page.Locator("#MenuRelated").GetByRole(AriaRole.Link, new() { Name = "Tags" }).Nth(0).ClickAsync();
                await page.GetByLabel("Tag Filter 1").FillAsync("TEST.Filler.");
                await page.GetByRole(AriaRole.Button, new() { Name = "Refresh" }).ClickAsync();
                await page.Locator("#MenuTagViewMode").GetByText("Updating").ClickAsync();

                //ASSERTIONS: confirm Script Class tag value is correctly updating as source tag values are editted
                //Test Case 1: running=1, temperature=80, expected: 'Not Overheated'
                await page.Locator("//*[contains(text(),'TEST.Filler.Running.Tag')]/ancestor::tr").GetByRole(AriaRole.Link, new() { Name = "Edit Value" }).ClickAsync();
                await page.GetByLabel("Value").FillAsync("1");
                await page.GetByRole(AriaRole.Link, new() { Name = "Save" }).ClickAsync();
                await page.Locator("//*[contains(text(),'TEST.Filler.Temperature.Tag')]/ancestor::tr").GetByRole(AriaRole.Link, new() { Name = "Edit Value" }).ClickAsync();
                await page.GetByLabel("Value").FillAsync("80");
                await page.GetByRole(AriaRole.Link, new() { Name = "Save" }).ClickAsync();
                await Expect(page.Locator("xpath=//*[contains(text(),'TEST.Filler.Overheated.Tag')]/ancestor::tr").GetByText("Not Overheated", new() { Exact = true })).ToBeVisibleAsync();
                await Screenshot(page);

                //Test Case 2: running=1, temperature=100, expected: 'Overheated'
                await page.Locator("//*[contains(text(),'TEST.Filler.Temperature.Tag')]/ancestor::tr").GetByRole(AriaRole.Link, new() { Name = "Edit Value" }).ClickAsync();
                await page.GetByLabel("Value").FillAsync("100");
                await page.GetByRole(AriaRole.Link, new() { Name = "Save" }).ClickAsync();
                await Expect(page.Locator("xpath=//*[contains(text(),'TEST.Filler.Overheated.Tag')]/ancestor::tr").GetByText("Overheated", new() { Exact = true })).ToBeVisibleAsync();
                await Screenshot(page);

                //Test Case 3: running=0, temperature=100, expected: 'Not Overheated'
                await page.Locator("//*[contains(text(),'TEST.Filler.Running.Tag')]/ancestor::tr").GetByRole(AriaRole.Link, new() { Name = "Edit Value" }).ClickAsync();
                await page.GetByLabel("Value").FillAsync("0");
                await page.GetByRole(AriaRole.Link, new() { Name = "Save" }).ClickAsync();
                await Expect(page.Locator("xpath=//*[contains(text(),'TEST.Filler.Overheated.Tag')]/ancestor::tr").GetByText("Not Overheated", new() { Exact = true })).ToBeVisibleAsync();
                await Screenshot(page);

                //clean up after test
                StopLogicService(); //stop logic service
                await page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Configuration']").ClickAsync();    //delete tags
                await page.GetByRole(AriaRole.Link, new() { Name = "Tags", Exact = true }).ClickAsync();
                await page.GetByText("TESTGROUP").ClickAsync();
                await page.GetByText("TESTAREA").ClickAsync();
                await page.GetByText("TESTLINE 1").ClickAsync();
                await page.Locator("#tsslice-index-2").GetByText("TEST.Filler.Overheated.Tag").ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
                await page.Locator("#tsslice-index-2").GetByText("TEST.Filler.Running.Tag").ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
                await page.Locator("#tsslice-index-2").GetByText("TEST.Filler.Temperature.Tag").ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
                await page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Developer']").ClickAsync();    //delete script class
                await page.GetByRole(AriaRole.Link, new() { Name = "Scripts" }).ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "TESTGROUP" }).ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "TEST_FillerOverheatedScript" }).ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
            });
        }

        [SetUp]
        public async Task Setup()
        {
            //playwright->browser->context->page: create new playwright instance, browser instance, set browser context via config file values, get new page instance
            var playwright = new PlaywrightManager(testSettings);
            page = playwright.Page;

            LoginPage loginPage = new LoginPage(page);
            await loginPage.Login("administrator", "Test123$$");
        }

        [TearDown]
        public async Task Teardown()
        {
            if ((bool)testSettings.Trace)
            {
                await PlaywrightManager.StopTracing(page.Context);
            }

            await page.Context.CloseAsync();
            await page.Context.Browser.CloseAsync();
        }
    }
}