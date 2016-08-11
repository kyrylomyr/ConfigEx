using System.ComponentModel;

namespace ConfigEx.TypeConverters
{
    public class TypeConverter : ITypeConverter
    {
        public virtual T Convert<T>(string value)
        {
            if (value == null)
            {
                return default(T);
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(value);
        }
    }
}
