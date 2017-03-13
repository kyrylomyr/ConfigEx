using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace ConfigEx
{
    public sealed class AssemblyConfigProvider : IConfigProvider
    {
        private readonly Assembly _assembly;

        public AssemblyConfigProvider(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly), "Assembly can not be null");
            }

            _assembly = assembly;
        }

        public Configuration Get()
        {
            var dllPath = new Uri(_assembly.GetName().CodeBase).LocalPath;
            var configuration = ConfigurationManager.OpenExeConfiguration(dllPath);
            if (configuration.HasFile)
            {
                return configuration;
            }

            throw new FileNotFoundException($"Failed to open config file '{configuration.FilePath}'");
        }
    }
}
