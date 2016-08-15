namespace ConfigEx
{
    public interface ITypeConverter
    {
        T Convert<T>(string value);
    }
}