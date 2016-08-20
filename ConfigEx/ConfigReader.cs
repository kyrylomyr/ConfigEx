using System;
using System.Configuration;
using System.Diagnostics.Contracts;

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
            Contract.Requires<ArgumentNullException>(localConfigProvider != null, "Local Config Provider can not be null");

            _localConfigurationLazy = new Lazy<KeyValueConfigurationCollection>(() => GetConfig(localConfigProvider));
            _mainConfigurationLazy = new Lazy<KeyValueConfigurationCollection>(() => GetConfig(mainConfigProvider));
        }

        public string Get(string key, bool overridable = true)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(key), "Key can not be null or empty");

            var localSetting = LocalConfiguration[key];
            if (localSetting == null)
            {
                return null;
            }

            // Do not read main configuration if the setting is not overridable or the main configuration is not provided.
            if (!overridable || MainConfiguration == null)
            {
                return localSetting.Value ?? string.Empty;
            }

            // Local setting can be overridden in the main configuration.
            var mainSetting = MainConfiguration[key];

            var value = mainSetting == null ? localSetting.Value : mainSetting.Value;
            return value ?? string.Empty;
        }

        private static KeyValueConfigurationCollection GetConfig(IConfigProvider configProvider)
        {
            if (configProvider == null)
            {
                return null;
            }

            var config = configProvider.Get();
            Contract.Requires<Exception>(config != null, "Config Provider returned null Configuration");

            var appSettings = config.AppSettings;
            Contract.Requires<Exception>(appSettings != null, $"Config file '{config.FilePath}' does not contain appSettings section");

            return appSettings.Settings;
        }
    }
}