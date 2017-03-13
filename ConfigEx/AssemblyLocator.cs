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
                       ? new ConfigAssemblyInfo(entryAssembly, ConfigAssemblyType.ClassLibrary)
                       : GetAssemblyWithAttribute();
        }

        private static ConfigAssemblyInfo GetAssemblyWithAttribute()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var entryAssemblies =
                (from asm in assemblies
                 let attr = asm.GetCustomAttributes(typeof(MainConfigAssemblyAttribute)).FirstOrDefault()
                 where attr != null
                 select new ConfigAssemblyInfo(asm, ((MainConfigAssemblyAttribute)attr).AssemblyType)).ToList();

            if (entryAssemblies.Count > 1)
            {
                throw new Exception("Only one assembly should be marked as main with the MainConfigAssemblyAttribute");
            }

            return entryAssemblies.Count == 1 ? entryAssemblies[0] : null;
        }
    }
}