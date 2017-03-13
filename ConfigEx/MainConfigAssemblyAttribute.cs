using System;

namespace ConfigEx
{
    /// <summary>
    /// Specifies the assembly that contains the main config.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class MainConfigAssemblyAttribute : Attribute
    {
        public ConfigAssemblyType AssemblyType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainConfigAssemblyAttribute"/> class.
        /// </summary>
        /// <param name="type">The config Assembly type.</param>
        public MainConfigAssemblyAttribute(ConfigAssemblyType type = ConfigAssemblyType.ClassLibrary)
        {
            AssemblyType = type;
        }
    }
}
