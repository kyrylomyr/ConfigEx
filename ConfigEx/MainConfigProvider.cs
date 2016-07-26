using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace ConfigEx
{
    public sealed class MainConfigProvider : IConfigProvider
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
                throw new FileNotFoundException($"Failed to open config file '{asm.Location}'", ex);
            }
        }
    }
}