using ConfigEx.Converters;
using ConfigEx.Providers;
using ConfigEx.Readers;

namespace ConfigEx
{
    /// <summary>
    /// Provides readers for main and local Assembly configurations.
    /// </summary>
    public sealed class Config
    {
        static Config()
        {
            var converter = new TypeConverter();
            Main = new AssemblyConfigReader(new MainAssemblyConfigProvider(), converter);
            Local = new AssemblyConfigReader(new LocalAssemblyConfigProvider(), converter);
        }

        /// <summary>
        /// The reader that reads values from the Entry Assembly configuration.
        /// </summary>
        public static IConfigReader Main { get; set; }

        /// <summary>
        /// The reader that reads values from the Calling Assembly configuration.
        /// </summary>
        public static IConfigReader Local { get; set; }
    }
}