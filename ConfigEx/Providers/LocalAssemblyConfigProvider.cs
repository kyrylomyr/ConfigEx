using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConfigEx.Providers
{
    public sealed class LocalAssemblyConfigProvider : IConfigProvider
    {
        public Configuration Get()
        {
            // Find first Assembly outside the current one - that is the Assembly from where the code is called.
            var currentAsm = Assembly.GetExecutingAssembly();
            var stackTrace = new StackTrace();
            var asm = stackTrace.GetFrames()?
                                .Select(x => x.GetMethod()?.ReflectedType?.Assembly)
                                .FirstOrDefault(x => x != currentAsm);

            if (asm == null)
            {
                throw new Exception("Can not find calling Assembly");
            }

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