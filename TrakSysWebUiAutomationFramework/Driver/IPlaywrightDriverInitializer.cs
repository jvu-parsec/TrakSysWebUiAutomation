using Microsoft.Playwright;
using TrakSysWebUiAutomationFramework.Config;

namespace TrakSysWebUiAutomationFramework.Driver
{
    public interface IPlaywrightDriverInitializer
    {
        Task<IBrowser> GetChromeDriverAsync(TestSettings settings);
        Task<IBrowser> GetChromiumDriverAsync(TestSettings settings);
        Task<IBrowser> GetEdgeDriverAsync(TestSettings settings);
        Task<IBrowser> GetFirefoxDriverAsync(TestSettings settings);
        Task<IBrowser> GetWebkitDriverAsync(TestSettings settings);
    }
}