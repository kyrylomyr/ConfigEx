namespace ConfigEx
{
    public interface IConfigReader
    {
        string Get(string key, bool overridable = true);
    }
}