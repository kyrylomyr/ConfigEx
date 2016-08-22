using System;
using System.Globalization;
using ConfigEx;

namespace MyApp.RefLib
{
    public class CustomConverter : TypeConverter
    {
        public override T Convert<T>(string value)
        {
            if (typeof(T) == typeof(DateTime))
            {
                return (T)(object)DateTime.ParseExact(value, "ddMMyyyy", CultureInfo.InvariantCulture);
            }

            return base.Convert<T>(value);
        }
    }
}