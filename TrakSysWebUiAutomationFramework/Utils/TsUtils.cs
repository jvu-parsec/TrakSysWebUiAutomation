using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace TsWebUiAutomationFramework.Utils
{
    public class TsUtils
    {
        public static void RestartLogicService()
        {
            string restartLogicScriptPath = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\PowershellScripts\\RestartLogicService.ps1";

            using (var ps = PowerShell.Create())
            {
                ps.AddScript($"start-process powershell -windowstyle hidden -verb runas -argumentlist '-file {restartLogicScriptPath}'");
                ps.Invoke();
            }
        }

        public static void StopLogicService()
        {
            string stopLogicServiceScriptPath = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\PowershellScripts\\StopLogicService.ps1";

            using (var ps = PowerShell.Create())
            {
                ps.AddScript($"start-process powershell -windowstyle hidden -verb runas -argumentlist '-file {stopLogicServiceScriptPath}'");
                ps.Invoke();
            }
        }
    }
}
