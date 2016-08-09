using System.Reflection;

namespace ConfigEx.AssemblyProviders
{
    public sealed class MainAssemblyProvider : IAssemblyProvider
    {
        public Assembly GetAssembly()
        {
            return Assembly.GetEntryAssembly();
        }
    }
}
