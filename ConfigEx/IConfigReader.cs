namespace ConfigEx
{
    public interface IConfigReader
    {
        string Get(string key);

        string GetOverridden(string key);
    }
}