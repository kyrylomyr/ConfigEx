namespace ConfigEx.Converters
{
    public interface ITypeConverter
    {
        T Convert<T>(string value);
    }
}