using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace ConfigEx.Providers
{
    public sealed class MainAssemblyConfigProvider : IConfigProvider
    {
        public Configuration Get()
        {
            var asm = Assembly.GetEntryAssembly();

            try
            {
                return ConfigurationManager.OpenExeConfiguration(asm.Location);
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"Failed to open config file of assembly '{asm.Location}'", ex);
            }
        }
    }
}