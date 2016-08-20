using System;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(localConfigProvider != null, "Local Config Provider can not be null");
            Contract.Requires<ArgumentNullException>(typeConverter != null, "Type Converter can not be null");

            Init(CreateConfigReader(localConfigProvider, mainConfigProvider), typeConverter);
        }

        protected ConfigBase(IConfigProvider localConfigProvider, IConfigProvider mainConfigProvider)
        {
            Contract.Requires<ArgumentNullException>(localConfigProvider != null, "Local Config Provider can not be null");

            Init(CreateConfigReader(localConfigProvider, mainConfigProvider), null);
        }

        protected ConfigBase(IConfigReader configReader, ITypeConverter typeConverter)
        {
            Contract.Requires<ArgumentNullException>(configReader != null, "Config Reader can not be null");
            Contract.Requires<ArgumentNullException>(typeConverter != null, "Type Converter can not be null");

            Init(configReader, typeConverter);
        }

        protected ConfigBase(ITypeConverter typeConverter)
        {
            Contract.Requires<ArgumentNullException>(typeConverter != null, "Type Converter can not be null");

            Init(null, typeConverter);
        }

        protected ConfigBase(IConfigReader configReader)
        {
            Contract.Requires<ArgumentNullException>(configReader != null, "Config Reader can not be null");

            Init(configReader, null);
        }

        protected ConfigBase()
        {
            Init(null, null);
        }

        protected T Get<T>(T defaultValue = default(T), bool overridable = true, [CallerMemberName] string key = null)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(key), "Key can not be null or empty");

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