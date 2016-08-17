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
            try
            {
                return ConfigurationManager.OpenExeConfiguration(_assembly.Location);
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"Failed to open config file of assembly '{_assembly.Location}'", ex);
            }
        }
    }
}
