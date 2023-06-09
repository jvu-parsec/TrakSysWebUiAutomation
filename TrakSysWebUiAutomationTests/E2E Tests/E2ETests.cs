using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using TSWebUiAutomationFramework.Driver;
using TSWebUiAutomationFramework.Config;

namespace TrakSysWebUiAutomationTests.E2E_Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class E2ETests
    {
        protected IPage page;

        [SetUp]
        public void Setup()
        {
            //playwright -> browser -> context -> page
            var testSettings = new TestSettings()
            {
                BrowserType = Browsers.Chromium,
                Timeout = 30000,
                Headless = false,
                SlowMo = 250,
                BaseUrl = "http://win-2u8u3utcd9b/TS/pages/home/"
            };

            var playwright = new PlaywrightManager(testSettings);
            page = playwright.Page;
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
    }
}