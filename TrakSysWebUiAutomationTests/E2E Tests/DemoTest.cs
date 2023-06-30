using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsWebUiAutomationTests.E2E_Tests
{
  public class DemoTest : PageTest
  {
    [SetUp]
    public virtual async Task SetupAsync()
    {
      await Page.Context.Tracing.StartAsync(new()
      {
        Title = $"{this.GetType().Name} - {TestContext.CurrentContext.Test.MethodName} on {DateTime.Now:g}",
        Screenshots = true,
        Snapshots = true,
        Sources = true
      });
    }

    [TearDown]
    public async Task TeardownAsync()
    {
      await Page.Context.Tracing.StopAsync(new()
      {
        Path = $"C:\\Users\\jvu\\source\\repos\\TrakSysWebUiAutomation\\TrakSysWebUiAutomationTests\\traces\\{this.GetType().Name}-{TestContext.CurrentContext.Test.MethodName}_{DateTime.Now:MM-dd-yy_HH_mm_ss}.zip"
      });

      await Page.Context.CloseAsync();
    }

    [Test]
    public async Task GoToReddit()
    {
      await Page.GotoAsync("https://www.reddit.com/");
      await Page.Locator("input#header-search-bar").HighlightAsync();
      Thread.Sleep(5000);
    }
  }
}
