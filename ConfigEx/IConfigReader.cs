namespace ConfigEx
{
    public interface IConfigReader
    {
        T Get<T>(string key, T defaultValue = default(T));
    }
}