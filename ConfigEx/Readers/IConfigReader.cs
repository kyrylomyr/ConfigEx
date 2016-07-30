namespace ConfigEx.Readers
{
    public interface IConfigReader
    {
        T Get<T>(string key, T defaultValue = default(T));
    }
}