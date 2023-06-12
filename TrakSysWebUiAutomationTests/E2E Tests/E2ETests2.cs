using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsWebUiAutomationFramework.Config;
using TsWebUiAutomationFramework.Driver;

namespace TsWebUiAutomationTests.E2E_Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class E2ETests2
    {
        private IPage page;

        [SetUp]
        public void Setup()
        {
            var testSettings = new TestSettings()
            {
                Browser = Browser.Chromium,
                Timeout = 30000,
                Headless = false,
                SlowMo = 250,
                BaseUrl = "http://win-2u8u3utcd9b/TS/pages/home/"
            };

            var playwright = new PlaywrightManager(testSettings);
            page = playwright.Page;

            page.Context.Tracing.StartAsync(new()
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        [TearDown]
        public async Task Teardown()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss fff");
            await page.Context.Tracing.StopAsync(new()
            {
                Path = $"traceviewer/{TestContext.CurrentContext.Test.MethodName} {timestamp} TRACE.zip",
            });

            await page.Context.CloseAsync();
            await page.Context.Browser.CloseAsync();
        }

        [Test, Retry(3)]
        public async Task Test_LoginAdminValidCredentials()
        {
            await page.GotoAsync("http://win-2u8u3utcd9b/TS/Account/LogOn.aspx/");

            //login form locators
            var txtUsername = page.GetByPlaceholder("Login");
            var txtPassword = page.GetByPlaceholder("Password");
            var chkRememberMe = page.GetByLabel("Remember Me");
            var btnSignIn = page.GetByRole(AriaRole.Button, new() { Name = "Sign In" });

            //fill out login form and submit/click login
            await txtUsername.TypeAsync("administrator");
            await txtPassword.TypeAsync("Test123$$");
            await chkRememberMe.ClickAsync();
            await btnSignIn.ClickAsync();

            await page.WaitForURLAsync("**/pages/home/**");
        }

        [Test, Retry(3)]
        public async Task Test_LoginAdminValidCredentials2()
        {
            await page.GotoAsync("http://win-2u8u3utcd9b/TS/Account/LogOn.aspx/");

            //login form locators
            var txtUsername = page.GetByPlaceholder("Login");
            var txtPassword = page.GetByPlaceholder("Password");
            var chkRememberMe = page.GetByLabel("Remember Me");
            var btnSignIn = page.GetByRole(AriaRole.Button, new() { Name = "Sign In" });

            //fill out login form and submit/click login
            await txtUsername.TypeAsync("administrator");
            await txtPassword.TypeAsync("Test123$$");
            await chkRememberMe.ClickAsync();
            await btnSignIn.ClickAsync();

            await page.WaitForURLAsync("**/pages/home/**");
        }
    }
}
