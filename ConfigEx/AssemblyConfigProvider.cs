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
                throw new ArgumentNullException(nameof(assembly), "Assembly can not be null");

            _assembly = assembly;
        }

        public Configuration Get()
        {
            try
            {
                var dllPath = new Uri(_assembly.GetName().CodeBase).LocalPath;
                return ConfigurationManager.OpenExeConfiguration(dllPath);
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"Failed to open config file of assembly '{_assembly.FullName}'", ex);
            }
        }
    }
}
