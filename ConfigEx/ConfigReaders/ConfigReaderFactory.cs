using ConfigEx.AssemblyProviders;
using ConfigEx.ConfigCache;
using ConfigEx.ConfigProviders;
using ConfigEx.TypeConverters;

namespace ConfigEx.ConfigReaders
{
    internal sealed class ConfigReaderFactory
    {
        public IConfigReader Create()
        {
            var assemblyProvider = new MainAssemblyProvider();
            var configCache = new AssemblyConfigCache();
            var configProvider = new AssemblyConfigProvider(assemblyProvider, configCache);
            var typeConverter = new TypeConverter();
            return Create(configProvider, typeConverter);
        }

        public IConfigReader Create(IAssemblyProvider assemblyProvider, ITypeConverter typeConverter = null)
        {
            var configCache = new AssemblyConfigCache();
            var configProvider = new AssemblyConfigProvider(assemblyProvider, configCache);
            return Create(configProvider, typeConverter);
        }

        public IConfigReader Create(IConfigProvider configProvider, ITypeConverter typeConverter = null)
        {
            return new ConfigReader(configProvider, typeConverter ?? new TypeConverter());
        }
    }
}
