namespace ConfigEx.ConfigReaders
{
    public interface IConfigReader
    {
        bool KeyExists(string key);

        T ReadSetting<T>(string key, T defaultValue = default(T));
    }
}