using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using TsWebUiAutomationFramework.Config;
using TsWebUiAutomationFramework.Driver;
using TsWebUiAutomationTests.Pages.Login;

namespace TsWebUiAutomationTests.E2E_Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class VisualPageTests : PageTest
    {
        private TestSettings testSettings = TestSettings.GetSettingsFromConfig();
        private IPage page;

        [Test]
        public async Task Test_CreateSimpleVisualPage()
        {
            await page.Locator("ul[class='navbar-nav d-xl-flex d-none'] a[title='Developer']").ClickAsync();

            await page.Locator("#tsslice-index-2").GetByRole(AriaRole.Link, new() { Name = "TEST HUB" }).ClickAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "New Hub 1" }).ClickAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "Visual" }).ClickAsync();

            await page.GetByRole(AriaRole.Listitem).Filter(new() { HasText = "Default Page" }).ClickAsync();

            await page.GetByRole(AriaRole.Button, new() { Name = "New" }).ClickAsync();

            await page.GetByLabel("Name", new() { Exact = true }).FillAsync("Visual Page Test 1");

            await page.GetByLabel("Key", new() { Exact = true }).FillAsync("VPT1");

            await page.GetByLabel("Page Title").FillAsync("Visual Page Test 1");

            await page.GetByLabel("Navigation Title").FillAsync("Visual Page Test 1");

            await page.GetByLabel("Refresh (Minutes)").FillAsync("1");

            await page.GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true }).ClickAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "Visual Page Test 1" }).ClickAsync();

            await page.Locator("#tsslice-index-3").GetByRole(AriaRole.Link, new() { Name = "Grid" }).ClickAsync();

            await page.PauseAsync();
        }

        [SetUp]
        public async Task Setup()
        {
            //playwright->browser->context->page: create new playwright instance, browser instance, set browser context via config file values, get new page instance
            var playwright = new PlaywrightManager(testSettings);
            page = await playwright.Page;

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
