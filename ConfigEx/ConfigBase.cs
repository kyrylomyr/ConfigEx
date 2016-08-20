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
            Contract.Requires<ArgumentNullException>(mainConfigProvider != null, "Main Config Provider can not be null");
            Contract.Requires<ArgumentNullException>(typeConverter != null, "Type Converter can not be null");

            Init(CreateConfigProvider(localConfigProvider, mainConfigProvider), typeConverter);
        }

        protected ConfigBase(IConfigProvider localConfigProvider, IConfigProvider mainConfigProvider)
        {
            Contract.Requires<ArgumentNullException>(localConfigProvider != null, "Local Config Provider can not be null");
            Contract.Requires<ArgumentNullException>(mainConfigProvider != null, "Main Config Provider can not be null");

            Init(CreateConfigProvider(localConfigProvider, mainConfigProvider), null);
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
            _configReader = configReader ?? CreateConfigProvider(null, null);
            _typeConverter = typeConverter ?? new TypeConverter();
        }

        private IConfigReader CreateConfigProvider(IConfigProvider localConfigProvider, IConfigProvider mainConfigProvider)
        {
            return new ConfigReader(
                localConfigProvider ?? new AssemblyConfigProvider(GetType().Assembly),
                mainConfigProvider ?? new AssemblyConfigProvider(Assembly.GetEntryAssembly()));
        }
    }
}