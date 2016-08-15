using System.Runtime.CompilerServices;

namespace ConfigEx
{
    public abstract class ConfigBase
    {
        private IConfigReader _configReader;
        private ITypeConverter _typeConverter;

        protected ConfigBase(IConfigReader configReader, ITypeConverter typeConverter)
        {
            _configReader = configReader;
            _typeConverter = typeConverter;
        }

        protected ConfigBase(ITypeConverter typeConverter)
        {
        }

        protected ConfigBase(IConfigReader configReader)
        {
        }

        protected ConfigBase()
        {
        }

        protected T Get<T>([CallerMemberName] string settingName = null, T defaultValue = default(T))
        {
            var value = _configReader.ReadSetting(settingName);
            return typeof(T) == typeof(string)
                       ? (T)(object)(value ?? string.Empty)
                       : _typeConverter.Convert<T>(value);
        }
    }
}