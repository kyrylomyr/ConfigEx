using System;
using System.Configuration;

namespace ConfigEx.ConfigCache
{
    public interface IConfigCache
    {
        Configuration GetOrAddConfig(string key, Func<Configuration> getConfig);
    }
}