using System.Reflection;

namespace ConfigEx.AssemblyProviders
{
    public sealed class MainAssemblyProvider : IAssemblyProvider
    {
        public Assembly Get()
        {
            return Assembly.GetEntryAssembly();
        }
    }
}
