using System.Configuration;
using ConfigEx.Converters;
using ConfigEx.Providers;

namespace ConfigEx.Readers
{
    /// <summary>
    /// The default implementation of <see cref="IConfigReader"/> that reads configuration settings from the
    /// <see cref="Configuration"/> provided by <see cref="IConfigProvider"/>.
    /// </summary>
    public sealed class AssemblyConfigReader : IConfigReader
    {
        private readonly IConfigProvider _configProvider;
        private readonly ITypeConverter _typeConverter;

        public AssemblyConfigReader(IConfigProvider configProvider, ITypeConverter typeConverter)
        {
            _configProvider = configProvider;
            _typeConverter = typeConverter;
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

            return _typeConverter.Convert<T>(value);
        }
    }
}