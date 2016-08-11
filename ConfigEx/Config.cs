using ConfigEx.AssemblyProviders;
using ConfigEx.ConfigProviders;
using ConfigEx.ConfigReaders;
using ConfigEx.TypeConverters;

namespace ConfigEx
{
    public static class Config
    {
        private static IConfigReader _mainConfigReader;
        private static IConfigReader _localConfigReader;

        static Config()
        {
            var factory = new ConfigReaderFactory();
            _mainConfigReader = factory.Create(new MainAssemblyProvider());
            _localConfigReader = factory.Create(new LocalAssemblyProvider());
        }

        public static T GetMainSetting<T>(string key, T defaultValue = default(T))
        {
            return _mainConfigReader.ReadSetting(key, defaultValue);
        }

        public static T GetLocalSetting<T>(string key, T defaultValue = default(T))
        {
            return _localConfigReader.ReadSetting(key, defaultValue);
        }

        public static T GetOverriddenSetting<T>(string key, T defaultValue = default(T))
        {
            return SettingIsOverridden(key) ? GetMainSetting(key, defaultValue) : GetLocalSetting(key, defaultValue);
        }

        public static bool MainSettingExists(string key)
        {
            return _mainConfigReader.KeyExists(key);
        }

        public static bool LocalSettingExists(string key)
        {
            return _localConfigReader.KeyExists(key);
        }

        public static bool SettingIsOverridden(string key)
        {
            return LocalSettingExists(key) && MainSettingExists(key);
        }

        public static void Init(ITypeConverter converter)
        {
            var factory = new ConfigReaderFactory();
            _mainConfigReader = factory.Create(converter);
        }

        public static void Init(IConfigReader mainReader, IConfigReader localReader)
        {
            _mainConfigReader = mainReader;
            _localConfigReader = localReader;
        }

        public static void Init(IConfigProvider mainProvider, IConfigProvider localProvider, ITypeConverter converter)
        {
            var factory = new ConfigReaderFactory();
            _mainConfigReader = factory.Create(mainProvider, converter);
            _localConfigReader = factory.Create(localProvider, converter);
        }

        public static void Init(IConfigProvider mainProvider, IConfigProvider localProvider)
        {
            var factory = new ConfigReaderFactory();
            _mainConfigReader = factory.Create(mainProvider);
            _localConfigReader = factory.Create(localProvider);
        }
    }
}