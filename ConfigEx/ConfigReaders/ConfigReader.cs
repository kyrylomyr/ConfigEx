using System.Configuration;
using ConfigEx.ConfigProviders;
using ConfigEx.TypeConverters;

namespace ConfigEx.ConfigReaders
{
    public sealed class ConfigReader : IConfigReader
    {
        private readonly IConfigProvider _configProvider;
        private readonly ITypeConverter _typeConverter;

        public ConfigReader(IConfigProvider configProvider, ITypeConverter typeConverter)
        {
            _configProvider = configProvider;
            _typeConverter = typeConverter;
        }

        public bool KeyExists(string key)
        {
            var config = _configProvider.Get();

            string value;
            return Exists(config, key, out value);
        }

        public T Get<T>(string key, T defaultValue = default(T))
        {
            var config = _configProvider.Get();

            string value;
            if (Exists(config, key, out value))
            {
                return typeof(T) == typeof(string)
                           ? (T)(object)(value ?? string.Empty)
                           : _typeConverter.Convert<T>(value);
            }

            return defaultValue;
        }

        private static bool Exists(Configuration config, string key, out string value)
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
    }
}