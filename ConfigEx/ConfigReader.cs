using System;
using System.Configuration;

namespace ConfigEx
{
    public sealed class ConfigReader : IConfigReader
    {
        private readonly Lazy<KeyValueConfigurationCollection> _localConfigurationLazy;
        private readonly Lazy<KeyValueConfigurationCollection> _mainConfigurationLazy;

        private KeyValueConfigurationCollection LocalConfiguration => _localConfigurationLazy.Value;
        private KeyValueConfigurationCollection MainConfiguration => _mainConfigurationLazy.Value;

        public ConfigReader(IConfigProvider localConfigProvider, IConfigProvider mainConfigProvider)
        {
            if (localConfigProvider == null)
            {
                throw new ArgumentNullException(nameof(localConfigProvider), "Local Config Provider can not be null");
            }

            _localConfigurationLazy = new Lazy<KeyValueConfigurationCollection>(() => GetConfig(localConfigProvider));
            _mainConfigurationLazy = new Lazy<KeyValueConfigurationCollection>(() => GetConfig(mainConfigProvider));
        }

        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key can not be null or empty", nameof(key));
            }

            var localSetting = LocalConfiguration[key];

            // If the setting is missing in the config then the setting is null and the value will be null.
            return localSetting == null ? null : GetSettingValue(localSetting);
        }

        public string GetOverridden(string key)
        {
            var localValue = Get(key);
            if (localValue == null || MainConfiguration == null)
            {
                // If local config doesn't have the setting then it can not be overridden in the main config.
                // Also doesn't make sense to check for overridden value if the main configuration was not provided.
                return null;
            }

            var mainSetting = MainConfiguration[key];

            // If the setting is missing in the main config then the local setting's value is not overridden.
            // Otherwise, return the value from the main config.
            return mainSetting == null ? localValue : GetSettingValue(mainSetting);
        }

        private static string GetSettingValue(KeyValueConfigurationElement setting)
        {
            // The null value is an empty string.
            return setting.Value ?? string.Empty;
        }

        private static KeyValueConfigurationCollection GetConfig(IConfigProvider configProvider)
        {
            if (configProvider == null)
            {
                return null;
            }

            var config = configProvider.Get();
            if (config == null)
            {
                throw new Exception("Config Provider returned null Configuration");
            }

            var settingsSection = config.AppSettings;
            if (settingsSection == null)
            {
                throw new Exception($"Config file '{config.FilePath}' does not contain appSettings section");
            }

            return settingsSection.Settings;
        }
    }
}