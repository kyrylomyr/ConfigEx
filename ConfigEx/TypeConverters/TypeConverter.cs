using System.ComponentModel;

namespace ConfigEx.TypeConverters
{
    public class TypeConverter : ITypeConverter
    {
        public virtual T Convert<T>(string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(value);
        }
    }
}
