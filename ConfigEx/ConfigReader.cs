using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace ConfigEx
{
    public sealed class ConfigReader : IConfigReader
    {
        private readonly Configuration _mainConfiguration;
        private readonly Configuration _localConfiguration;

        public ConfigReader()
        {
            _mainConfiguration = GetConfig(Assembly.GetEntryAssembly());
            _localConfiguration = GetConfig(Assembly.GetCallingAssembly());
        }

        public bool SettingExists(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key can not be null or empty", nameof(key));
            }

            string value;
            return SettingExists(_localConfiguration, key, out value);
        }

        public string ReadSetting(string key, string defaultValue = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key can not be null or empty", nameof(key));
            }

            string localConfigValue;
            if (SettingExists(_localConfiguration, key, out localConfigValue))
            {
                string mainConfigValue;
                if (SettingExists(_mainConfiguration, key, out mainConfigValue))
                {
                    return mainConfigValue;
                }

                return localConfigValue;
            }

            return defaultValue;
        }

        private static bool SettingExists(Configuration config, string key, out string value)
        {
            var setting = config.AppSettings.Settings[key];
            if (setting != null)
            {
                value = setting.Value;
                return true;
            }

            value = null;
            return false;
        }

        private static Configuration GetConfig(Assembly assembly)
        {
            try
            {
                return ConfigurationManager.OpenExeConfiguration(assembly.Location);
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"Failed to open config file of assembly '{assembly.Location}'", ex);
            }
        }
    }
}