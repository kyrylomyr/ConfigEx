using ConfigEx.ConfigProviders;
using ConfigEx.ConfigReaders;
using ConfigEx.TypeConverters;

namespace ConfigEx
{
    public static class Config
    {
        private static IConfigReader _mainConfigReader;
        private static IConfigReader _localConfigReader;

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

        public static void Init(IConfigReader mainConfigReader, IConfigReader localConfigReader)
        {
        }

        public static void Init(IConfigProvider mainConfigProvider, IConfigProvider localConfigProvider, ITypeConverter typeConverter)
        {

        }

        public static void Init(IConfigProvider mainConfigProvider, IConfigProvider localConfigProvider)
        {

        }

        public static void Init(ITypeConverter typeConverter)
        {

        }
    }
}