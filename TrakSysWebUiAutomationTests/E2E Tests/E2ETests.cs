using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using TrakSysWebUiAutomationFramework.Driver;

namespace TrakSysWebUiAutomationTests.E2E_Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class E2ETests
    {
        private PlaywrightDriver playwrightDriver;


        [SetUp]
        public void Setup()
        {
            playwrightDriver = new PlaywrightDriver();
        }

        [Test, Retry(3)]
        public async Task Test_LoginAdminValidCredentials()
        {
            var page = playwrightDriver.Page;

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