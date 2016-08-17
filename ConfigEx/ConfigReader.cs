using System;
using System.Configuration;
using System.Diagnostics.Contracts;

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
            Contract.Requires<ArgumentNullException>(mainConfigProvider != null, "Main Config Provider can not be null");
            Contract.Requires<ArgumentNullException>(localConfigProvider != null, "Local Config Provider can not be null");

            _mainConfigurationLazy = new Lazy<Configuration>(() => GetConfig(mainConfigProvider));
            _localConfigurationLazy = new Lazy<Configuration>(() => GetConfig(localConfigProvider));
        }

        public string Get(string key)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(key), "Key can not be null or empty");

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

        private static Configuration GetConfig(IConfigProvider configProvider)
        {
            var config = configProvider.Get();
            Contract.Requires<ArgumentNullException>(config != null, "Config Provider returned null Configuration");
            return config;
        }
    }
}