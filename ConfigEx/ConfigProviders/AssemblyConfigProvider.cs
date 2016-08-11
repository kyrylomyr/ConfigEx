using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using ConfigEx.AssemblyProviders;
using ConfigEx.ConfigCache;

namespace ConfigEx.ConfigProviders
{
    public sealed class AssemblyConfigProvider : IConfigProvider
    {
        private readonly IAssemblyProvider _assemblyProvider;
        private readonly IConfigCache _configCache;

        public AssemblyConfigProvider(IAssemblyProvider assemblyProvider, IConfigCache configCache = null)
        {
            if (assemblyProvider == null)
            {
                throw new ArgumentNullException(nameof(assemblyProvider), "Assembly Provider can not be null");
            }

            _assemblyProvider = assemblyProvider;
            _configCache = configCache;
        }

        public Configuration GetConfig()
        {
            var assembly = _assemblyProvider.GetAssembly();
            if (assembly == null)
            {
                throw new Exception("Assembly, returned by provider, can not be null");
            }

            return _configCache == null
                       ? GetActualConfig(assembly)
                       : _configCache.GetOrAddConfig(assembly.FullName, () => GetActualConfig(assembly));
        }

        private static Configuration GetActualConfig(Assembly assembly)
        {
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