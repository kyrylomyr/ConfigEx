using System.Reflection;

namespace ConfigEx.AssemblyProviders
{
    public interface IAssemblyProvider
    {
        Assembly Get();
    }
}