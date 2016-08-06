using System;
using System.Collections.Concurrent;
using System.Configuration;

namespace ConfigEx.ConfigCache
{
    public sealed class AssemblyConfigCache : IConfigCache
    {
        private readonly ConcurrentDictionary<string, Configuration> _cache =
            new ConcurrentDictionary<string, Configuration>();

        public Configuration GetOrAdd(string fullAssemblyName, Func<Configuration> getConfig)
        {
            return _cache.GetOrAdd(fullAssemblyName, getConfig());
        }
    }
}