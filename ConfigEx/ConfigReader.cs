using System;
using System.Configuration;

namespace ConfigEx
{
    public sealed class ConfigReader : IConfigReader
    {
        private readonly Lazy<Configuration> _mainConfigurationLazy;
        private readonly Lazy<Configuration> _localConfigurationLazy;

        private Configuration MainConfiguration => _mainConfigurationLazy.Value;
        private Configuration LocalConfiguration => _localConfigurationLazy.Value;

        public ConfigReader(IConfigProvider mainConfigProvider, IConfigProvider localConfigProvider)
        {
            if (mainConfigProvider == null)
            {
                throw new ArgumentNullException(nameof(mainConfigProvider), "Main Config Provider can not be null");
            }

            if (localConfigProvider == null)
            {
                throw new ArgumentNullException(nameof(localConfigProvider), "Local Config Provider can not be null");
            }

            _mainConfigurationLazy = new Lazy<Configuration>(mainConfigProvider.Get);
            _localConfigurationLazy = new Lazy<Configuration>(localConfigProvider.Get);
        }

        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key can not be null or empty", nameof(key));
            }

            var localSetting = LocalConfiguration.AppSettings.Settings[key];
            if (localSetting == null)
            {
                return null;
            }

            // Local setting can be overridden in the main configuration.
            var mainSetting = MainConfiguration.AppSettings.Settings[key];

            var value = mainSetting == null ? localSetting.Value : mainSetting.Value;
            return value ?? string.Empty;
        }
    }
}