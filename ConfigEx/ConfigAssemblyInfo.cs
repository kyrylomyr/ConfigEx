using System.Reflection;

namespace ConfigEx
{
    internal class ConfigAssemblyInfo
    {
        public ConfigAssemblyInfo(Assembly assembly, ConfigAssemblyType type)
        {
            Assembly = assembly;
            Type = type;
        }

        public Assembly Assembly { get; set; }

        public ConfigAssemblyType Type { get; set; }
    }
}