namespace ConfigEx
{
    public interface IConfigReader
    {
        bool SettingExists(string key);

        string ReadSetting(string key, string defaultValue = null);
    }
}