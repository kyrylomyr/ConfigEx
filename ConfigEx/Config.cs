using ConfigEx.AssemblyProviders;
using ConfigEx.ConfigProviders;
using ConfigEx.ConfigReaders;
using ConfigEx.TypeConverters;

namespace ConfigEx
{
    public static class Config
    {
        private static readonly ConfigReaderFactory _readerFactory = new ConfigReaderFactory();

        private static IConfigReader _mainConfigReader;
        private static IConfigReader _localConfigReader;

        static Config()
        {
            _mainConfigReader = _readerFactory.Create(new MainAssemblyProvider());
            _localConfigReader = _readerFactory.Create(new LocalAssemblyProvider());
        }

        #region Single config

        public static T GetSetting<T>(string key, T defaultValue = default(T))
        {
            return _mainConfigReader.ReadSetting(key, defaultValue);
        }

        public static bool SettingExists(string key)
        {
            return _mainConfigReader.KeyExists(key);
        }

        #endregion

        #region Main and Local configs

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

        #endregion

        #region Init

        public static void Init(IConfigReader configReader)
        {
            _mainConfigReader = configReader;
        }

        public static void Init(IConfigProvider configProvider, ITypeConverter typeConverter)
        {
            _mainConfigReader = _readerFactory.Create(configProvider, typeConverter);
        }

        public static void Init(IConfigProvider configProvider)
        {
            _mainConfigReader = _readerFactory.Create(configProvider);
        }

        public static void Init(ITypeConverter typeConverter)
        {
            _mainConfigReader = _readerFactory.Create(typeConverter);
        }

        public static void Init(IConfigReader mainConfigReader, IConfigReader localConfigReader)
        {
            _mainConfigReader = mainConfigReader;
            _localConfigReader = localConfigReader;
        }

        public static void Init(
            IConfigProvider mainConfigProvider, IConfigProvider localConfigProvider, ITypeConverter typeConverter)
        {
            _mainConfigReader = _readerFactory.Create(mainConfigProvider, typeConverter);
            _localConfigReader = _readerFactory.Create(localConfigProvider, typeConverter);
        }

        public static void Init(IConfigProvider mainConfigProvider, IConfigProvider localConfigProvider)
        {
            _mainConfigReader = _readerFactory.Create(mainConfigProvider);
            _localConfigReader = _readerFactory.Create(localConfigProvider);
        }

        #endregion
    }
}