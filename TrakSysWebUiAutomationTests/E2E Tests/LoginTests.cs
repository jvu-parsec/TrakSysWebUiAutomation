using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TsWebUiAutomationFramework.Config;
using TsWebUiAutomationFramework.Driver;
using TsWebUiAutomationTests.Pages.Login;

namespace TsWebUiAutomationTests.E2E_Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class LoginTests : PageTest
    {
        private TestSettings testSettings = TestSettings.GetSettingsFromConfig();
        private IPage page;

        [Test, Retry(3)]
        public async Task Test1_LoginAdminValidCredentials()
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

        [SetUp]
        public async Task Setup()
        {
            //playwright->browser->context->page: create new playwright instance, browser instance, set browser context via config file values, get new page instance
            var playwright = new PlaywrightManager(testSettings);
            page = await playwright.Page;
            
            await page.GotoAsync(testSettings.BaseUrl + "account/logon.aspx");

            LoginPage loginPage = new LoginPage(page);
            await loginPage.Login("administrator", "Test!23");
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
