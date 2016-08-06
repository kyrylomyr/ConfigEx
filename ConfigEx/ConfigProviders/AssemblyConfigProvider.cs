﻿using System;
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

        public AssemblyConfigProvider(IAssemblyProvider assemblyProvider, IConfigCache configCache)
        {
            _assemblyProvider = assemblyProvider;
            _configCache = configCache;
        }

        public Configuration Get()
        {
            var assembly = _assemblyProvider.Get();
            if (assembly == null)
            {
                throw new Exception("Assembly, returned by provider, can not be null");
            }

            return _configCache.GetOrAdd(assembly.FullName, () => GetActualConfig(assembly));
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