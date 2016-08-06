using System;
using System.Configuration;

namespace ConfigEx.ConfigCache
{
    public interface IConfigCache
    {
        Configuration GetOrAdd(string key, Func<Configuration> getConfig);
    }
}