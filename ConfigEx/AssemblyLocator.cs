using System;
using System.Linq;
using System.Reflection;

namespace ConfigEx
{
    internal static class AssemblyLocator
    {
        public static ConfigAssemblyInfo GetEntryAssembly()
        {
            // Entry assembly is null in the ASP.NET and test projects. In this case we can mark the right assembly
            // with the MainConfigAssemblyAttribute and find it by the attribute.
            var entryAssembly = Assembly.GetEntryAssembly();
            return entryAssembly != null
                       ? new ConfigAssemblyInfo(entryAssembly, ConfigAssemblyType.TestMainAssembly)
                       : GetAssemblyWithAttribute();
        }

        private static ConfigAssemblyInfo GetAssemblyWithAttribute()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var entryAsms = (from asm in assemblies
                             let attr = asm.GetCustomAttributes(typeof(MainConfigAssemblyAttribute)).FirstOrDefault()
                             where attr != null
                             select new ConfigAssemblyInfo(asm, ((MainConfigAssemblyAttribute)attr).AssemblyType)).ToList();

            if (entryAsms.Count > 1)
            {
                // Check for the assemblies with the Test AssemblyType.
                var testAsms = entryAsms.Where(x => x.Type == ConfigAssemblyType.TestMainAssembly).ToList();
                if (testAsms.Count == 1)
                {
                    return testAsms.First();
                }
                
                throw new Exception("Only one assembly should be marked as main with the MainConfigAssemblyAttribute");
            }

            return entryAsms.Count == 1 ? entryAsms[0] : null;
        }
    }
}