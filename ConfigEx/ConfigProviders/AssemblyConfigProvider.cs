using System;
using System.Configuration;
using System.IO;
using ConfigEx.AssemblyProviders;

namespace ConfigEx.ConfigProviders
{
    public sealed class AssemblyConfigProvider : IConfigProvider
    {
        private readonly IAssemblyProvider _assemblyProvider;

        public AssemblyConfigProvider(IAssemblyProvider assemblyProvider)
        {
            _assemblyProvider = assemblyProvider;
        }

        public Configuration Get()
        {
            var assembly = _assemblyProvider.Get();
            if (assembly == null)
            {
                throw new Exception("Assembly, returned by provider, can not be null");
            }

            try
            {
                return ConfigurationManager.OpenExeConfiguration(assembly.Location);
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"Failed to open config file of assembly '{assembly.Location}'", ex);
            }
        }
    }
}