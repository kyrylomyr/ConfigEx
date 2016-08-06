namespace ConfigEx.TypeConverters
{
    public interface ITypeConverter
    {
        T Convert<T>(string value);
    }
}