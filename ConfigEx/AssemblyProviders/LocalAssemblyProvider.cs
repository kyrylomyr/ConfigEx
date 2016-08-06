using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ConfigEx.AssemblyProviders
{
    public sealed class LocalAssemblyProvider : IAssemblyProvider
    {
        public Assembly Get()
        {
            // Find first Assembly outside the current one - that is the Assembly from where the code is called.
            var currentAssembly = Assembly.GetExecutingAssembly();
            var stackTrace = new StackTrace();
            var assembly = stackTrace.GetFrames()?
                                     .Select(x => x.GetMethod()?.ReflectedType?.Assembly)
                                     .FirstOrDefault(x => x != currentAssembly);

            if (assembly == null)
            {
                throw new Exception("Can not find local Assembly");
            }

            return assembly;
        }
    }
}