using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsWebUiAutomationTests.Pages;

namespace TsWebUiAutomationTests.Pages.Login
{
    public class LoginPage : BasePage
    {
        private readonly IPage _page;

        public LoginPage(IPage page) : base(page) 
        {
            _page = page;
        }

        private ILocator _txtUsername => _page.GetByLabel("Login");
        private ILocator _txtPassword => _page.GetByLabel("Password");
        private ILocator _chkRememberMe => _page.GetByLabel("Remember Me");
        private ILocator _btnSignIn => _page.GetByRole(AriaRole.Button, new() { Name = "Sign In" });

        public async Task Login(string username, string password)
        {
            //await _page.GotoAsync("http://win-2u8u3utcd9b/TS/Account/LogOn.aspx/");

            await _txtUsername.FillAsync(username);
            await _txtPassword.FillAsync(password);
            await _chkRememberMe.ClickAsync();
            await _btnSignIn.ClickAsync();
        }
    }
}
