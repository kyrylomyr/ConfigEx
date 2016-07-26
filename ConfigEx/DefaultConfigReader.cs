using System.ComponentModel;
using System.Configuration;

namespace ConfigEx
{
    /// <summary>
    /// The default implementation of <see cref="IConfigReader"/> that reads configuration settings from the
    /// <see cref="Configuration"/> provided by <see cref="IConfigProvider"/>.
    /// </summary>
    public sealed class DefaultConfigReader : IConfigReader
    {
        private readonly IConfigProvider _configProvider;

        public DefaultConfigReader(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }

        /// <summary>
        /// Reads the configuration setting value by the specified key.
        /// </summary>
        /// <typeparam name="T">The type of configuration setting value</typeparam>
        /// <param name="key">The name of the configuration setting</param>
        /// <param name="defaultValue">The default value for the case when setting with the specified key does not exist</param>
        /// <returns>The setting value; or default value if setting with the specified key does not exist.</returns>
        public T Get<T>(string key, T defaultValue = default(T))
        {
            var config = _configProvider.Get();

            // Read value.
            var value = config.AppSettings.Settings[key]?.Value;

            if (typeof(T) == typeof(string))
            {
                return value == null ? defaultValue : (T)(object)value;
            }

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            // Convert value from string to target type.
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(value);
        }
    }
}