namespace ConfigEx.ConfigReaders
{
    public interface IConfigReader
    {
        bool KeyExists(string key);

        T Get<T>(string key, T defaultValue = default(T));
    }
}