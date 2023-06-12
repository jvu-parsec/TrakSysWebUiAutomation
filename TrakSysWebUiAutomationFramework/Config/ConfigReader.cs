using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsWebUiAutomationFramework.Config
{
    public class ConfigReader
    {
        public string configFilePath = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\testsettings.config";


        public ConfigReader() { }

        public ConfigReader(string configFileName)
        {
            configFilePath = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\{configFileName}";
        }

        public string GetSetting(string key)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFilePath;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            return config.AppSettings.Settings[key].Value;
        }

    }
}
