using ConfigEx.AssemblyProviders;
using ConfigEx.ConfigProviders;
using ConfigEx.ConfigReaders;
using ConfigEx.TypeConverters;

namespace ConfigEx
{
    public sealed class Config
    {
        private static IConfigReader _mainConfigReader;
        private static IConfigReader _localConfigReader;

        static Config()
        {
            var converter = new TypeConverter();

            var mainProvider = new AssemblyConfigProvider(new MainAssemblyProvider());
            var localProvider = new AssemblyConfigProvider(new LocalAssemblyProvider());

            _mainConfigReader = new ConfigReader(mainProvider, converter);
            _localConfigReader = new ConfigReader(localProvider, converter);
        }
    }
}