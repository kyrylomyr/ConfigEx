using System;
using System.Collections.Concurrent;
using System.Configuration;

namespace ConfigEx.ConfigCache
{
    public sealed class AssemblyConfigCache : IConfigCache
    {
        private readonly ConcurrentDictionary<string, Configuration> _cache =
            new ConcurrentDictionary<string, Configuration>();

        public Configuration GetOrAddConfig(string key, Func<Configuration> getConfig)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key can not be null or empty", nameof(key));
            }

            if (getConfig == null)
            {
                throw new ArgumentException("Function that returns Configuration can not be null", nameof(getConfig));
            }

            return _cache.GetOrAdd(key, getConfig());
        }
    }
}