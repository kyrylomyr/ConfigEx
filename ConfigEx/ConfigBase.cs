using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ConfigEx
{
    public abstract class ConfigBase
    {
        private IConfigReader _configReader;
        private ITypeConverter _typeConverter;

        protected ConfigBase(IConfigProvider localConfigProvider, IConfigProvider mainConfigProvider, ITypeConverter typeConverter)
        {
            if (localConfigProvider == null)
                throw new ArgumentNullException(nameof(localConfigProvider), "Local Config Provider can not be null");
            if (typeConverter == null)
                throw new ArgumentNullException(nameof(typeConverter), "Type Converter can not be null");

            Init(CreateConfigReader(localConfigProvider, mainConfigProvider), typeConverter);
        }

        protected ConfigBase(IConfigProvider localConfigProvider, IConfigProvider mainConfigProvider)
        {
            if (localConfigProvider == null)
                throw new ArgumentNullException(nameof(localConfigProvider), "Local Config Provider can not be null");

            Init(CreateConfigReader(localConfigProvider, mainConfigProvider), null);
        }

        protected ConfigBase(IConfigReader configReader, ITypeConverter typeConverter)
        {
            if (configReader == null)
                throw new ArgumentNullException(nameof(configReader), "Config Reader can not be null");
            if (typeConverter == null)
                throw new ArgumentNullException(nameof(typeConverter), "Type Converter can not be null");

            Init(configReader, typeConverter);
        }

        protected ConfigBase(ITypeConverter typeConverter)
        {
            if (typeConverter == null)
                throw new ArgumentNullException(nameof(typeConverter), "Type Converter can not be null");

            Init(null, typeConverter);
        }

        protected ConfigBase(IConfigReader configReader)
        {
            if (configReader == null)
                throw new ArgumentNullException(nameof(configReader), "Config Reader can not be null");

            Init(configReader, null);
        }

        protected ConfigBase()
        {
            Init(null, null);
        }

        protected T Get<T>(T defaultValue = default(T), bool overridable = true, [CallerMemberName] string key = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key can not be null or empty", nameof(key));

            var value = _configReader.Get(key, overridable);
            return value == null ? defaultValue : _typeConverter.Convert<T>(value);
        }

        private void Init(IConfigReader configReader, ITypeConverter typeConverter)
        {
            _configReader = configReader ?? CreateConfigReader(CreateLocalConfigProvider(), CreateMainConfigProvider());
            _typeConverter = typeConverter ?? new TypeConverter();
        }

        private static IConfigReader CreateConfigReader(IConfigProvider localConfigProvider, IConfigProvider mainConfigProvider)
        {
            return new ConfigReader(localConfigProvider, mainConfigProvider);
        }

        private IConfigProvider CreateLocalConfigProvider()
        {
            // The current instance of ConfigBase derived type is expected to be located in the Assembly which config should be read.
            // Thus, the Assembly of the current type is used for local config provider. It always exists.
            return new AssemblyConfigProvider(GetType().Assembly);
        }

        private static IConfigProvider CreateMainConfigProvider()
        {
            // Main config provider should be initialized with the default AssemblyConfigProvider and entry Assembly.
            // Entry Assembly can be null, and in this case the config provider should not be created.
            var entryAssembly = Assembly.GetEntryAssembly();
            return entryAssembly != null ? new AssemblyConfigProvider(entryAssembly) : null;
        }
    }
}