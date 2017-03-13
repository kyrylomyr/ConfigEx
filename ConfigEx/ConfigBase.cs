using System;
using System.Runtime.CompilerServices;

namespace ConfigEx
{
    public abstract class ConfigBase
    {
        private IConfigReader _configReader;
        private ITypeConverter _typeConverter;

        protected ConfigBase(
            IConfigProvider localConfigProvider, IConfigProvider mainConfigProvider, ITypeConverter typeConverter)
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

        protected T Get<T>(T defaultValue = default(T), [CallerMemberName] string key = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key can not be null or empty", nameof(key));

            var value = _configReader.Get(key);
            return value == null ? defaultValue : _typeConverter.Convert<T>(value);
        }

        protected T GetOverridden<T>(T defaultValue = default(T), [CallerMemberName] string key = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key can not be null or empty", nameof(key));

            var value = _configReader.GetOverridden(key);
            return value == null ? defaultValue : _typeConverter.Convert<T>(value);
        }

        private void Init(IConfigReader configReader, ITypeConverter typeConverter)
        {
            _configReader = configReader ?? CreateConfigReader();
            _typeConverter = typeConverter ?? new TypeConverter();
        }

        private static IConfigReader CreateConfigReader(
            IConfigProvider localConfigProvider, IConfigProvider mainConfigProvider)
        {
            return new ConfigReader(localConfigProvider, mainConfigProvider);
        }

        private IConfigReader CreateConfigReader()
        {
            // The current instance of ConfigBase derived type is expected to be in the Assembly which config should
            // be read. Thus, the Assembly of the current type is used for the local config provider. It always exists.
            var localAssembly = GetType().Assembly;

            // However, it can be the main assembly of Web project. Thus, we need to read the config another way - as
            // a Web.config. So, first need to find the main assembly and compare with the current one.
            var mainAssemblyInfo = AssemblyLocator.GetEntryAssembly();
            if (mainAssemblyInfo == null)
            {
                // The main assembly can be null. In that case it will be possible to read only the local config.
                // If the main config is a Web.config, the reader will fail - the main assembly should be marked
                // with the MainConfigAssemblyAttribute.
                return new ConfigReader(new AssemblyConfigProvider(localAssembly), null);
            }

            // If the local and main assemblies are the same assemblies, then create config provider according
            // to the assembly type. The main config provider is not needed in this case.
            if (localAssembly.FullName == mainAssemblyInfo.Assembly.FullName)
            {
                return mainAssemblyInfo.Type == ConfigAssemblyType.ClassLibrary
                           ? new ConfigReader(new AssemblyConfigProvider(localAssembly), null)
                           : new ConfigReader(new WebConfigProvider(), null);
            }

            // If the local and main assemblies are different, then the local config is a regular App.config, and the
            // main config should be read according to the main assembly type.
            return mainAssemblyInfo.Type == ConfigAssemblyType.ClassLibrary
                           ? new ConfigReader(
                               new AssemblyConfigProvider(localAssembly),
                               new AssemblyConfigProvider(mainAssemblyInfo.Assembly))
                           : new ConfigReader(
                               new AssemblyConfigProvider(localAssembly),
                               new WebConfigProvider());
        }
    }
}