using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrakSysWebUiAutomationFramework.Driver;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace TrakSysWebUiAutomationTests.E2E_Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class E2ETests2
    {
        private PlaywrightDriver playwrightDriver;
        private IPage page;

        [SetUp]
        public void Setup()
        {
            playwrightDriver = new PlaywrightDriver();
            page = playwrightDriver.Page;
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
