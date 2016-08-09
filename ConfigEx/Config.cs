using ConfigEx.ConfigProviders;
using ConfigEx.ConfigReaders;
using ConfigEx.TypeConverters;

namespace ConfigEx
{
    public static class Config
    {
        private static IConfigReader _mainConfigReader;
        private static IConfigReader _localConfigReader;

        public static T GetMain<T>(string key, T defaultValue = default(T))
        {
            return _mainConfigReader.Get(key, defaultValue);
        }

        public static T GetLocal<T>(string key, T defaultValue = default(T))
        {
            return _localConfigReader.Get(key, defaultValue);
        }

        public static T GetOverridden<T>(string key, T defaultValue = default(T))
        {
            return IsOverridden(key) ? GetMain(key, defaultValue) : GetLocal(key, defaultValue);
        }

        public static bool MainExists(string key)
        {
            return _mainConfigReader.KeyExists(key);
        }

        public static bool LocalExists(string key)
        {
            return _localConfigReader.KeyExists(key);
        }

        public static bool IsOverridden(string key)
        {
            return LocalExists(key) && MainExists(key);
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