using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ConfigEx
{
    public abstract class ConfigBase
    {
        private IConfigReader _configReader;
        private ITypeConverter _typeConverter;

        protected ConfigBase(
            IConfigProvider mainConfigProvider, IConfigProvider localConfigProvider, ITypeConverter typeConverter)
        {
            if (mainConfigProvider == null)
            {
                throw new ArgumentNullException(nameof(mainConfigProvider), "Main Config Provider can not be null");
            }

            if (localConfigProvider == null)
            {
                throw new ArgumentNullException(nameof(localConfigProvider), "Local Config Provider can not be null");
            }

            if (typeConverter == null)
            {
                throw new ArgumentNullException(nameof(typeConverter), "Type Converter can not be null");
            }

            Init(CreateConfigProvider(mainConfigProvider, localConfigProvider), typeConverter);
        }

        protected ConfigBase(IConfigProvider mainConfigProvider, IConfigProvider localConfigProvider)
        {
            if (mainConfigProvider == null)
            {
                throw new ArgumentNullException(nameof(mainConfigProvider), "Main Config Provider can not be null");
            }

            if (localConfigProvider == null)
            {
                throw new ArgumentNullException(nameof(localConfigProvider), "Local Config Provider can not be null");
            }

            Init(CreateConfigProvider(mainConfigProvider, localConfigProvider), null);
        }

        protected ConfigBase(IConfigReader configReader, ITypeConverter typeConverter)
        {
            if (configReader == null)
            {
                throw new ArgumentNullException(nameof(configReader), "Config Reader can not be null");
            }

            if (typeConverter == null)
            {
                throw new ArgumentNullException(nameof(typeConverter), "Type Converter can not be null");
            }

            Init(configReader, typeConverter);
        }

        protected ConfigBase(ITypeConverter typeConverter)
        {
            if (typeConverter == null)
            {
                throw new ArgumentNullException(nameof(typeConverter), "Type Converter can not be null");
            }

            Init(null, typeConverter);
        }

        protected ConfigBase(IConfigReader configReader)
        {
            if (configReader == null)
            {
                throw new ArgumentNullException(nameof(configReader), "Config Reader can not be null");
            }

            Init(configReader, null);
        }

        protected ConfigBase()
        {
            Init(null, null);
        }

        protected T Get<T>([CallerMemberName] string key = null, T defaultValue = default(T))
        {
            var value = _configReader.Get(key);
            return value == null ? defaultValue : _typeConverter.Convert<T>(value);
        }

        private void Init(IConfigReader configReader, ITypeConverter typeConverter)
        {
            _configReader = configReader ?? CreateConfigProvider(null, null);
            _typeConverter = typeConverter ?? new TypeConverter();
        }

        private IConfigReader CreateConfigProvider(
            IConfigProvider mainConfigProvider, IConfigProvider localConfigProvider)
        {
            return new ConfigReader(
                mainConfigProvider ?? new AssemblyConfigProvider(Assembly.GetEntryAssembly()),
                localConfigProvider ?? new AssemblyConfigProvider(GetType().Assembly));
        }
    }
}