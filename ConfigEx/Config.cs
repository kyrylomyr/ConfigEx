using System;
using ConfigEx.AssemblyProviders;
using ConfigEx.ConfigCache;
using ConfigEx.ConfigProviders;
using ConfigEx.ConfigReaders;
using ConfigEx.TypeConverters;

namespace ConfigEx
{
    public static class Config
    {
        private static readonly Lazy<IConfigReader> _lazyMainConfigReader =
            new Lazy<IConfigReader>(InitMainConfigReader);

        private static readonly Lazy<IConfigReader> _lazyLocalConfigReader =
            new Lazy<IConfigReader>(InitLocalConfigReader);

        private static ITypeConverter _typeConverter;

        private static IConfigReader MainConfigReader => _lazyMainConfigReader.Value;
        private static IConfigReader LocalConfigReader => _lazyLocalConfigReader.Value;

        public static void SetTypeConverter(ITypeConverter typeConverter)
        {
            if (typeConverter == null)
            {
                throw new ArgumentNullException(nameof(typeConverter), "Type Converter can not be null");
            }

            if (_lazyMainConfigReader.IsValueCreated || _lazyLocalConfigReader.IsValueCreated)
            {
                throw new ArgumentException(
                    "Can not set Type Converter for already initialized Config Readers. Type Converter can be set before any Config read operation is done.");
            }

            TypeConverter = typeConverter;
        }

        public static ITypeConverter TypeConverter
        {
            private get { return _typeConverter; }
            set { _typeConverter = value; }
        }

        private static IConfigReader InitMainConfigReader()
        {
            var assemblyProvider = new MainAssemblyProvider();
            return CreateDefaultConfigReader(assemblyProvider);
        }

        private static IConfigReader InitLocalConfigReader()
        {
            var assemblyProvider = new LocalAssemblyProvider();
            return CreateDefaultConfigReader(assemblyProvider);
        }

        private static IConfigReader CreateDefaultConfigReader(IAssemblyProvider assemblyProvider)
        {
            var configCache = new AssemblyConfigCache();

            var mainProvider = new AssemblyConfigProvider(assemblyProvider, configCache);
            var typeConverter = TypeConverter ?? new TypeConverter();

            return new ConfigReader(mainProvider, typeConverter);
        }
    }
}